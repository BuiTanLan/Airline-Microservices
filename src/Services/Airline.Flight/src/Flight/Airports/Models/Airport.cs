using BuildingBlocks.Domain.Model;
using BuildingBlocks.IdsGenerator;
using Flight.Airports.Events;

namespace Flight.Airports.Models;

public class Airport : Aggregate<long>
{
    public Airport()
    {
    }

    public string Name { get; private set; }
    public string Address { get; private set; }
    public string Code { get; private set; }

    public static Airport Create(long id, string name, string address, string code)
    {
        var airport = new Airport
        {
            Id = id,
            Name = name,
            Address = address,
            Code = code
        };

        var @event = new AirportCreatedDomainEvent(
            airport.Id,
            airport.Name,
            airport.Address,
            airport.Code);

        airport.AddDomainEvent(@event);
        airport.Apply(@event);

        return airport;
    }

    public override void When(object @event)
    {
        switch (@event)
        {
            case AirportCreatedDomainEvent airportCreated:
            {
                Apply(airportCreated);
                return;
            }
        }
    }

    private void Apply(AirportCreatedDomainEvent @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Address = @event.Address;
        Code = @event.Code;
        Version++;
    }
}
