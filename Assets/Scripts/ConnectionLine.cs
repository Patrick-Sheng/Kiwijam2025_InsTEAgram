using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionLine : MonoBehaviour, IHitReceiver
{
    public LineRenderer lineRenderer;
    public int lineId;
    public Action<int> onRemoveEvent;

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
        onRemoveEvent.Invoke(lineId);
    }
}
