using UnityEngine;

[CreateAssetMenu(fileName = "RelationshipType", menuName = "GameData/RelationshipType", order = 1)]
public class RelationshipType : ScriptableObject
{
    public string type;
    public Color color;

    public RelationshipType(string type, Color color)
    {
        this.type = type;
        this.color = color;
    }
}
