using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float minDistance = 0.1f;
    public Vector3 prevPos;
    public List<Vector2> points = new List<Vector2>();
    

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        prevPos = transform.position;
    }

    public void DrawLine()
    {
        Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        if (Vector2.Distance(currentPosition, prevPos) > minDistance)
        {
            if (prevPos == transform.position)
            {
                lineRenderer.SetPosition(0, currentPosition);
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
                points.Add(currentPosition);
            }
            prevPos = currentPosition;

            //lineRenderer.positionCount++;
            //lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
            //points.Add(currentPosition);
            //prevPos = currentPosition;
        }
    }

    public void ResetPoints()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    public List<Vector2> GetPoints()
    {
        return points;
    }

    public void GenerateCollider()
    {
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, Camera.main, true);
        meshCollider.sharedMesh = mesh;
    }
}
