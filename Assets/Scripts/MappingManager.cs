using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Radishmouse;
using UnityEngine;

public class MappingManager : MonoBehaviour
{
    [SerializeField] private Dictionary<int, PairingInfo> connections; // Use int here instead of Guid cause we need it to be based on the pair
    [SerializeField] private Dictionary<Guid, ConnectionLine> connectionLines;

    [SerializeField] private PersonNode[] personNodes;

    [SerializeField] private PersonNode selectedNode;
    [SerializeField] private PersonNode targetNode;

    [SerializeField] private ConnectionLine line;

    [SerializeField] private RelationshipType selectedRelation = RelationshipTypes.CHEATING;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedNode != null)
            {
                selectedNode.DeactivateAimLine();
                selectedNode = null;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // For line renderer lines because the dynamic collider is in 3D
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("Different layer");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            // For everything else using a 2D collider
            if (hit)
            {
                //IHitReceiver hitObject = hit.collider.GetComponent<IHitReceiver>();
                PersonNode hitNode = hit.collider.GetComponent<PersonNode>();
                if (hitNode != null)
                {
                    if (selectedNode == null)
                    {
                        hitNode.OnRaycastHit();
                    }
                    SelectNode(hitNode);
                }
            }

        }
    }

    public void SelectNode(PersonNode node)
    {
        if (selectedNode == null)
        {
            selectedNode = node;
        }
        else if (node != selectedNode) // Connecting two nodes together
        {
            targetNode = node;

            // Create pairing uuid
            int pairingId = CreatePairingId(selectedNode.id, targetNode.id);

            // Check if pairing exists in colllection   
            if (connections.ContainsKey(pairingId))
            {
                // If it does exist, retrieve data, and check whether the relationship type exists in the relationship
                PairingInfo existingPair;
                connections.TryGetValue(pairingId, out existingPair);

                if (existingPair != null)
                {
                    // Check if max relationships between pair is existing (2)
                    if (existingPair.relationships.Count == 2)
                    {
                        // TODO: Prompt user
                        return;
                    }

                    // If the relationship does exist, don't add it and be done
                    foreach (Relationship ship in existingPair.relationships)
                    {
                        if (ship.relationshipType == selectedRelation)
                        {
                            return;
                        }
                    }

                    Relationship newRelationship = new Relationship(Guid.NewGuid(), selectedRelation);

                    // If there are not two relatipnships with the pair already, add the relationship and supply unique id
                    existingPair.relationships.Add(newRelationship);

                    DrawConnectionLine(existingPair, newRelationship);
                }
            }
            else
            {
                // Create relationship and add to collection
                PairingInfo newPairingInfo;
                Relationship newRelationship = new Relationship(Guid.NewGuid(), selectedRelation);
                newPairingInfo = new PairingInfo
                {
                    pair = Tuple.Create(selectedNode, targetNode),
                    relationships = new List<Relationship> { newRelationship }
                };

                DrawConnectionLine(newPairingInfo, newRelationship);
            }



        }
    }

    private void DrawConnectionLine(PairingInfo pairingInfo, Relationship newRelationship)
    {
        // ==== Line Creation ====

        // After adding relationship, instantiate ConnectionLine and initialise with callback and id
        ConnectionLine connLine = Instantiate(line);
        connLine.lineRenderer.startColor = selectedRelation.color;
        connLine.lineRenderer.endColor = selectedRelation.color;
        connLine.onRemoveEvent += RemoveLine;

        // Add new connectin line to connection line dictionary using relation ship id as the key
        connectionLines.Add(newRelationship.id, connLine);

        // Check the number of relationships for that pair
        // If the number is 1, just set line position using both the PersonNodes' origin points
        if (pairingInfo.relationships.Count == 1)
        {
            SetLinePoints(
                connLine,
                pairingInfo.pair.Item1.transform.position,
                pairingInfo.pair.Item2.transform.position
            );
        }
        else if (pairingInfo.relationships.Count == 2)
        {

            // If the number is 2, use special parallel point calculation to get relative points

            // Get actual points by adding relative points to PersonNode 1 & 2's positions

            // Retrieve existing line from connection line dictionary using relationship id and update it's position

            // Set the new connection line's positions as well
        }
    }

    private ConnectionLine CreateSingleLine(Vector2 pos1, Vector2 pos2)
    {
        ConnectionLine connLine = Instantiate(line);
        connLine.lineRenderer.SetPosition(0, pos1);
        connLine.lineRenderer.SetPosition(1, pos2);
        return connLine;
    }

    private void SetLinePoints(ConnectionLine line, Vector2 pos1, Vector2 pos2)
    {
        line.lineRenderer.SetPosition(0, pos1);
        line.lineRenderer.SetPosition(1, pos2);
    }

    private int CreatePairingId(int id1, int id2)
    {
        string idString1 = id1.ToString();
        string idString2 = id2.ToString();

        return int.Parse(idString1 + idString2);
    }

    private void RemoveLine(Guid lineId)
    {
        ConnectionLine line;
        connectionLines.TryGetValue(lineId, out line);
        Destroy(line.gameObject);
        connectionLines.Remove(lineId);
    }

    //public bool HasRelationWithEachOther(PersonNode to, PersonNode from)
    //{
    //    return to.connections.ContainsKey(from) && from.connections.ContainsKey(to);
    //}

    //public bool HasSameRelation(PersonNode to, PersonNode from, Relationship relationship)
    //{
    //    Relationship toShip;
    //    Relationship fromShip;
    //    to.connections.TryGetValue(from, out toShip);
    //    from.connections.TryGetValue(to, out fromShip);

    //    return toShip.type == fromShip.type;
    //}
}
