namespace BuildingBlocks.EventStoreDB.Core.Projections;

public interface IProjection
{
    void When(object @event);
}
