using BuildingBlocks.IdsGenerator;
using MediatR;

namespace Flight.Airports.Features.CreateAirport;

public record CreateAirportCommand(string Name, string Address, string Code) : IRequest<ulong>
{
    public long Id { get; set; } = SnowFlakIdGenerator.NewId();
}
