namespace BuildingBlocks.Domain.Model;

public abstract class Entity<TId> : IEntity<TId>, IAuditable
{
    protected Entity() { }

    public TId Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
}
