using System;
using UnityEngine;

public class RelationshipTypes
{
    public static RelationshipType DATING = new RelationshipType("Dating", Color.cyan);
    public static RelationshipType HOOKUP = new RelationshipType("Hookup", new Color(0.5803922f, 0f, 0.8274511f, 1f));
    public static RelationshipType HATES = new RelationshipType("Hates", Color.red);
    public static RelationshipType LIKES = new RelationshipType("Likes", Color.blue);
    public static RelationshipType CHEATING = new RelationshipType("Cheating", new Color(1f, 0.4117647f, 0.7058824f, 1f));
    public static RelationshipType EXES = new RelationshipType("Exes", Color.yellow);
}

public class RelationshipType
{
    public string type;
    public Color color;

    public RelationshipType(string type, Color color)
    {
        this.type = type;
        this.color = color;
    }
}

public class Relationship
{
    public Guid id { get; private set; }
    public RelationshipType relationshipType { get; private set; }

    public Relationship(Guid id, RelationshipType relationshipType)
    {
        this.id = id;
        this.relationshipType = relationshipType;
    }
}