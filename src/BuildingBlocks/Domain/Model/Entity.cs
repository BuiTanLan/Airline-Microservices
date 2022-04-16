namespace BuildingBlocks.Domain.Model;

public abstract class Entity<TId> : IEntity<TId>
{
    protected Entity() { }

    public TId Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public int? LastModifiedBy { get; set; }
}
