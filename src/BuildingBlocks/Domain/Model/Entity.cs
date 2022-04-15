namespace BuildingBlocks.Domain.Model;

public abstract class Entity
{
    protected Entity() { }
    public DateTime LastModified { get; protected set;}
    public bool IsDeleted { get; protected set;}
    public int? ModifiedBy { get; protected set;}
}
