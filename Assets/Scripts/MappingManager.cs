using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Radishmouse;
using UnityEngine;

public class MappingManager : MonoBehaviour
{
    [SerializeField] private Dictionary<int, PairingInfo> connections = new Dictionary<int, PairingInfo>(); // Use int here instead of Guid cause we need it to be based on the pair
    [SerializeField] private Dictionary<Guid, ConnectionLine> connectionLines = new Dictionary<Guid, ConnectionLine>();

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
                    Debug.Log("Existing pair found");
                    // Check if max relationships between pair is existing (2)
                    if (existingPair.relationships.Count == 2)
                    {
                        // TODO: Prompt user
                        EndSelection();
                        return;
                    }

                    // If the relationship does exist, don't add it and be done
                    foreach (Relationship ship in existingPair.relationships)
                    {
                        if (ship.relationshipType == selectedRelation)
                        {
                            EndSelection();
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
                connections.Add(pairingId, newPairingInfo);

                DrawConnectionLine(newPairingInfo, newRelationship);
            }
            EndSelection();
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

        Tuple<Vector2, Vector2> pairPostions = Tuple.Create(
            (Vector2)pairingInfo.pair.Item1.aimLineOrigin.position,
            (Vector2)pairingInfo.pair.Item2.aimLineOrigin.position);

        // Check the number of relationships for that pair
        // If the number is 1, just set line position using both the PersonNodes' origin points
        if (pairingInfo.relationships.Count == 1)
        {
            SetLinePoints(
                connLine,
                pairPostions.Item1,
                pairPostions.Item2
            );
        }
        else if (pairingInfo.relationships.Count == 2)
        {
            Debug.Log("Should do new calc");

            // If the number is 2, use special parallel point calculation to get relative points
            PointFinder pfinder = new PointFinder();
            (Vector2 relPoint1, Vector2 relPoint2) = pfinder.CalculateRelativeLineOriginPosition(
                pairPostions.Item1,
                pairPostions.Item2
            );

            // Get actual points by adding relative points to PersonNode 1 & 2's positions
            Vector2 actualPoint1ForP1 = pairPostions.Item1 + relPoint1;
            Vector2 actualPoint2ForP1 = pairPostions.Item1 + relPoint2;

            Vector2 actualPoint1ForP2 = pairPostions.Item2 + relPoint1;
            Vector2 actualPoint2ForP2 = pairPostions.Item2 + relPoint2;

            // Retrieve existing line from connection line dictionary using relationship id and update it's position
            ConnectionLine line1;
            connectionLines.TryGetValue(pairingInfo.relationships[0].id, out line1);

            ConnectionLine line2;
            connectionLines.TryGetValue(pairingInfo.relationships[1].id, out line2);

            // Set the new connection line's positions as well
            if (line1 != null && line2 != null)
            {
                SetLinePoints(line1, actualPoint1ForP1, actualPoint1ForP2);
                SetLinePoints(line2, actualPoint2ForP1, actualPoint2ForP2);
            }         
        }
    }

    private void EndSelection()
    {
        selectedNode.DeactivateAimLine();
        selectedNode = null;
        targetNode = null;
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

        int min = Mathf.Min(id1, id2);
        int max = Mathf.Max(id1, id2);
        Debug.Log((min, max).GetHashCode());
        return (min, max).GetHashCode();

        //string idString1 = id1.ToString();
        //string idString2 = id2.ToString();

        //return int.Parse(idString1 + idString2);
    }

    private void RemoveLine(Guid lineId)
    {
        ConnectionLine line;
        connectionLines.TryGetValue(lineId, out line);
        Destroy(line.gameObject);
        connectionLines.Remove(lineId);
    }

    public void SetRelationshipType() //TODO: Test for now, fix later
    {
        if(selectedRelation != RelationshipTypes.CHEATING)
        {
            selectedRelation = RelationshipTypes.CHEATING;
        }
        else
        {
            selectedRelation = RelationshipTypes.HOOKUP;
        }
    }
}
