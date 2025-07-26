using UnityEngine;

public class PointFinder : MonoBehaviour
{
    public Transform canvasParent;

    public float radiusModifier = 0.3f;

    public (Vector2, Vector2) CalculateRelativeLineOriginPosition(Vector2 pos1, Vector2 pos2)
    {
        Vector2 direction = (pos2 - pos1).normalized;
        Vector2 counterClockwise = new Vector2(-direction.y, direction.x);
        Vector2 clockwise = new Vector2(direction.y, -direction.x);
        return (counterClockwise * radiusModifier, clockwise * radiusModifier);
    }
}