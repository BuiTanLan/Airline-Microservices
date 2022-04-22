using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Flight.Data;
using Flight.Flights.Models.Reads;
using Mapster;
using MediatR;

namespace Flight.Flights.Features.CreateFlight.Read;

public class CreateFlightMongoReadModelHandler : IRequestHandler<CreateFlightMongoReadModel, Unit>
{
    private readonly FlightReadDbContext _flightReadDbContext;

    public CreateFlightMongoReadModelHandler(FlightReadDbContext flightReadDbContext)
    {
        _flightReadDbContext = flightReadDbContext;
    }

    public async Task<Unit> Handle(CreateFlightMongoReadModel command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var flight = command.Adapt<FlightReadModel>();

        await _flightReadDbContext.Flight.InsertOneAsync(flight, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
