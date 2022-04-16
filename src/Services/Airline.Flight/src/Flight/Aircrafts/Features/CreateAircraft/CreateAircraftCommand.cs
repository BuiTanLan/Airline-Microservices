using BuildingBlocks.IdsGenerator;
using MediatR;

namespace Flight.Aircrafts.Features.CreateAircraft;

public record CreateAircraftCommand(string Name, string Model, int ManufacturingYear) : IRequest<ulong>
{
    public long Id { get; set; } = SnowFlakIdGenerator.NewId();
}
