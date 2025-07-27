using UnityEngine;

// TODO: Remove?
public class RelationshipTypes
{
    public static RelationshipType DATING = new RelationshipType("Dating", Color.cyan);
    public static RelationshipType HOOKUP = new RelationshipType("Hookup", new Color(0.5803922f, 0f, 0.8274511f, 1f));
    public static RelationshipType HATES = new RelationshipType("Hates", Color.red);
    public static RelationshipType LIKES = new RelationshipType("Likes", Color.blue);
    public static RelationshipType CHEATING = new RelationshipType("Cheating", new Color(1f, 0.4117647f, 0.7058824f, 1f));
    public static RelationshipType EXES = new RelationshipType("Exes", Color.yellow);
}