using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Radishmouse;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MappingManager : MonoBehaviour
{
    [SerializeField] private Dictionary<int, PairingInfo> connections = new Dictionary<int, PairingInfo>(); // Use int here instead of Guid cause we need it to be based on the pair
    [SerializeField] private Dictionary<Guid, ConnectionLine> connectionLines = new Dictionary<Guid, ConnectionLine>();

    [SerializeField] private RelationshipButton[] relationshipBtns;

    [SerializeField] private PersonNode selectedNode;
    [SerializeField] private PersonNode targetNode;

    [SerializeField] private ConnectionLine line;

    [SerializeField] private RelationshipType selectedRelation;

    public void Start()
    {
        // Have the first button enabled by default
        relationshipBtns[0].EnableSprite();
        selectedRelation = relationshipBtns[0].relationshipType;
    }

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
                IHitReceiver hitReceiver = hitInfo.transform.GetComponent<IHitReceiver>();
                if (hitReceiver != null)
                {
                    hitReceiver.OnRaycastHit();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            // For everything else using a 2D collider
            if (hit)
            {
                PersonNode hitNode = hit.collider.GetComponent<PersonNode>();
                if (hitNode != null)
                {
                    if (selectedNode == null)
                    {
                        hitNode.SetLineColor(selectedRelation.color);
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
            BlockButtonUsage();
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

                    DrawConnectionLine(pairingId, existingPair, newRelationship);
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

                DrawConnectionLine(pairingId, newPairingInfo, newRelationship);
            }
            EndSelection();
        }
    }

    private void DrawConnectionLine(int pairId, PairingInfo pairingInfo, Relationship newRelationship)
    {
        // After adding relationship, instantiate ConnectionLine and initialise with callback and id
        ConnectionLine connLine = Instantiate(line);
        Debug.Log(selectedRelation.color);
        connLine.lineRenderer.startColor = selectedRelation.color;
        connLine.lineRenderer.endColor = selectedRelation.color;
        connLine.lineId = newRelationship.id;
        connLine.pairId = pairId;
        connLine.onRemoveEvent += RemoveRelationship;

        // Add new connectin line to connection line dictionary using relationship id as the key
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
                line1.RemakeMesh();
                line2.RemakeMesh();
            }         
        }
    }

    private void EndSelection()
    {
        selectedNode.DeactivateAimLine();
        selectedNode = null;
        targetNode = null;
        RenableButtonUsage();
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
        return (min, max).GetHashCode();
    }

    private void RemoveRelationship(int pairId, Guid lineId)
    {
        // Remove the line
        ConnectionLine line;
        connectionLines.TryGetValue(lineId, out line);

        Destroy(line.gameObject);
        connectionLines.Remove(lineId);

        // Remove reference in data
        PairingInfo info;
        connections.TryGetValue(pairId, out info);

        Relationship shipToRemove = null;
        foreach(Relationship ship in info.relationships)
        {
            if (ship.id == lineId)
            {
                shipToRemove = ship;
            }
        }
        info.relationships.Remove(shipToRemove);
        if (info.relationships.Count < 1)
        {
            connections.Remove(pairId);
        }
        else
        {
            // Shift position of line to center if there's still one relationship connection
            ConnectionLine remainingLine;
            connectionLines.TryGetValue(info.relationships[0].id, out remainingLine);
            SetLinePoints(
                remainingLine,
                info.pair.Item1.aimLineOrigin.position,
                info.pair.Item2.aimLineOrigin.position
            );
            remainingLine.RemakeMesh();
        }


    }

    public void OnButtonClickEvent(RelationshipButton button)
    {
        foreach(RelationshipButton btn in relationshipBtns)
        {
            btn.DisableSprite();
        }
        button.EnableSprite();
        SetRelationshipType(button.relationshipType);
    }

    public void BlockButtonUsage() // So they can't switch while the thread is aiming
    {
        foreach (RelationshipButton btn in relationshipBtns)
        {
            btn.GetComponent<Button>().interactable = false;
        }
    }

    public void RenableButtonUsage()
    {
        foreach (RelationshipButton btn in relationshipBtns)
        {
            btn.GetComponent<Button>().interactable = true;
        }
    }

    private void SetRelationshipType(RelationshipType type)
    {
        selectedRelation = type;
    }
}
