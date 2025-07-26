using UnityEngine;

public class PointFinder : MonoBehaviour
{
    public Transform canvasParent;

    public float radiusModifier = 1f;

    /*void CalculateAndSpawnPoints(Vector2 pos1, Vector2 pos2)
    {
        Vector2 direction = (pos2 - pos1).normalized;
        Vector2 counterClockwise = new Vector2(-direction.y, direction.x);
        Vector2 clockwise = new Vector2(direction.y, -direction.x);

        Vector2 newPosition1CounterClockwise = (Vector2)p1.position + counterClockwise * radiusModifier;
        Vector2 newPosition1Clockwise = (Vector2)p1.position + clockwise * radiusModifier;

        Vector2 newPosition2CounterClockwise = (Vector2)p2.position + counterClockwise * radiusModifier;
        Vector2 newPosition2Clockwise = (Vector2)p2.position + clockwise * radiusModifier;

        Instantiate(spawnObj, newPosition1CounterClockwise, Quaternion.identity, canvasParent);
        Instantiate(spawnObj, newPosition1Clockwise, Quaternion.identity, canvasParent);

        Instantiate(spawnObj, newPosition2CounterClockwise, Quaternion.identity, canvasParent);
        Instantiate(spawnObj, newPosition2Clockwise, Quaternion.identity, canvasParent);
    }*/

    void CalculateAndSpawnPoints(Vector2 pos1, Vector2 pos2)
    {
        Vector2 direction = (pos2 - pos1).normalized;
        Vector2 counterClockwise = new Vector2(-direction.y, direction.x);
        Vector2 clockwise = new Vector2(direction.y, -direction.x);

        Vector2 newPosition1CounterClockwise = pos1 + counterClockwise * radiusModifier;
        Vector2 newPosition1Clockwise = pos1 + clockwise * radiusModifier;

        Vector2 newPosition2CounterClockwise = pos2 + counterClockwise * radiusModifier;
        Vector2 newPosition2Clockwise = pos2 + clockwise * radiusModifier;
    }

    (Vector2, Vector2) CalculateRelativeLineOriginPosition(Vector2 pos1, Vector2 pos2)
    {
        Vector2 direction = (pos2 - pos1).normalized;
        Vector2 counterClockwise = new Vector2(-direction.y, direction.x);
        Vector2 clockwise = new Vector2(direction.y, -direction.x);
        return (counterClockwise, clockwise);
    }
}