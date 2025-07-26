using System.Collections.Generic;
using Radishmouse;
using UnityEngine;

public class MappingManager : MonoBehaviour
{
    [SerializeField] private Dictionary<int, PairingInfo> connections;
    [SerializeField] private Dictionary<int, ConnectionLine> connectionLines;

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
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            // For line renderer lines because the dynamic collider is in 3D
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("Different layer");
            }

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

            // Check if pairing exists in colllection

            // If it doesn't exist, add it and be done

            // If it does exist, retrieve data, and check whether the relationship type exists in the relationship

            // If the relationship does exist, don't add it and be done

            // If the relationship type doesn't exist, check if there are 2 relationships (cause thats the max between a pair)

            // If there are two already, don't add and be done (probably hint user for reason)

            // If there are not two already, add the relationship and supply unique id

            // ==== Line Creation ====

            // After adding relationship, instantiate ConnectionLine and initialise with callback and id

            // Add new connectin line to connection line dictionary using relation ship id as the key

            // Check the number of relationships for that pair

            // If the number is 2, use special parallel point calculation to get relative points

            // Get actual points by adding relative points to PersonNode 1 & 2's positions

            // Retrieve existing line from connection line dictionary using relationship id and update it's position

            // Set the new connection line's positions as well

            // If the number is 1, just set line position using both the PersonNodes' origin points
        }
    }

    private ConnectionLine CreateSingleLine(Vector2 pos1, Vector2 pos2)
    {
        ConnectionLine connLine = Instantiate(line);
        connLine.lineRenderer.SetPosition(0, pos1);
        connLine.lineRenderer.SetPosition(1, pos2);
        return connLine;
    }

    public bool HasRelationWithEachOther(PersonNode to, PersonNode from)
    {
        return to.connections.ContainsKey(from) && from.connections.ContainsKey(to);
    }

    public bool HasSameRelation(PersonNode to, PersonNode from, Relationship relationship)
    {
        Relationship toShip;
        Relationship fromShip;
        to.connections.TryGetValue(from, out toShip);
        from.connections.TryGetValue(to, out fromShip);

        return toShip.type == fromShip.type;
    }
}
