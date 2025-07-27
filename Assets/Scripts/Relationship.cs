using System;

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