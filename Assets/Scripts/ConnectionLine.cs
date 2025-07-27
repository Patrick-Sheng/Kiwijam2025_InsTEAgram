using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionLine : MonoBehaviour, IHitReceiver
{
    public LineRenderer lineRenderer;
    public int pairId;
    public Guid lineId;
    public Action<int, Guid> onRemoveEvent;

    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, Camera.main, true);
        meshCollider.sharedMesh = mesh;
    }

    public void OnRaycastHit()
    {
        onRemoveEvent.Invoke(pairId, lineId);
    }

    public void RemakeMesh()
    {
        lineRenderer = GetComponent<LineRenderer>();

        MeshCollider existingMeshCollider = GetComponent<MeshCollider>();
        if (existingMeshCollider != null)
        {
            Destroy(existingMeshCollider);
        }

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, Camera.main, true);
        meshCollider.sharedMesh = mesh;
    }
}
