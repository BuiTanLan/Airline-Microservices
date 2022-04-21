using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Flight.Flights.Features.CreateFlight.Read;

public class CreateFlightMongoReadModelHandler : IRequestHandler<CreateFlightMongoReadModel, Unit>
{
    public Task<Unit> Handle(CreateFlightMongoReadModel request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
