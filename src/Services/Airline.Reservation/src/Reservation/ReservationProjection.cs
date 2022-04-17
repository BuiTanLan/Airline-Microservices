using BuildingBlocks.EventStoreDB.Events;
using BuildingBlocks.EventStoreDB.Projections;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Reservations.Events.Domain;

namespace Reservation;

public class ReservationProjection : IProjection
{
    private readonly ReservationDbContext _reservationDbContext;

    public ReservationProjection(ReservationDbContext reservationDbContext)
    {
        _reservationDbContext = reservationDbContext;
    }

    public async Task ProcessEventAsync<T>(StreamEvent<T> streamEvent, CancellationToken cancellationToken = default)
        where T : INotification
    {
        switch (streamEvent.Data)
        {
            case ReservationCreatedDomainEvent reservationCreatedDomainEvent:
                await Apply(reservationCreatedDomainEvent, cancellationToken);
                break;
        }
    }

    private async Task Apply(ReservationCreatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var reservation =
            await _reservationDbContext.Reservations.SingleOrDefaultAsync(x => x.Id == @event.Id && !x.IsDeleted,
                cancellationToken);

        if (reservation == null)
        {
            var model = Reservations.Models.Reservation.Create(@event.Id, @event.PassengerInfo, @event.Trip);

            await _reservationDbContext.Set<Reservations.Models.Reservation>().AddAsync(model, cancellationToken);
            await _reservationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
