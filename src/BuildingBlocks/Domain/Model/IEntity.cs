namespace BuildingBlocks.Domain.Model;

public interface IEntity<out TId>
{
    TId Id { get; }
    public bool IsDeleted { get; }
    DateTime? CreatedAt { get; }
    int? CreatedBy { get; }
    DateTime? LastModified { get; }
    int? LastModifiedBy { get; }
}
