using Ardalis.GuardClauses;
using BuildingBlocks.Contracts.Grpc;
using BuildingBlocks.EventStoreDB.Repository;
using Grpc.Net.Client;
using MagicOnion.Client;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Reservation.Configuration;
using Reservation.Data;
using Reservation.Reservations.Dtos;
using Reservation.Reservations.Exceptions;
using Reservation.Reservations.Models.ValueObjects;

namespace Reservation.Reservations.Features.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ulong>
{
    private readonly IMapper _mapper;
    private readonly IEventStoreDBRepository<Models.Reservation> _eventStoreDbRepository;
    private readonly IFlightGrpcService _flightGrpcService;
    private readonly IPassengerGrpcService _passengerGrpcService;


    public CreateReservationCommandHandler(
        IOptions<GrpcOptions> grpcOptions,
        IMapper mapper,
        IEventStoreDBRepository<Models.Reservation> eventStoreDbRepository)
    {
        _mapper = mapper;
        _eventStoreDbRepository = eventStoreDbRepository;

        var channelFlight = GrpcChannel.ForAddress(grpcOptions.Value.FlightAddress);
        _flightGrpcService = new Lazy<IFlightGrpcService>(() => MagicOnionClient.Create<IFlightGrpcService>(channelFlight)).Value;

        var channelPassenger = GrpcChannel.ForAddress(grpcOptions.Value.PassengerAddress);
        _passengerGrpcService = new Lazy<IPassengerGrpcService>(() => MagicOnionClient.Create<IPassengerGrpcService>(channelPassenger)).Value;
    }

    public async Task<ulong> Handle(CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var flight = await _flightGrpcService.GetById(command.FlightId);
        if (flight is null)
            throw new FlightNotFoundException();
        var passenger = await _passengerGrpcService.GetById(command.PassengerId);

        var emptySeat = (await _flightGrpcService.GetAvailableSeats(command.FlightId))?.First();

        var reservation = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (reservation is not null && !reservation.IsDeleted)
            throw new ReservationAlreadyExistException();

        var aggrigate = Models.Reservation.Create(command.Id, new PassengerInfo(passenger.Name), new Trip(
            flight.FlightNumber, flight.AircraftId, flight.DepartureAirportId,
            flight.ArriveAirportId, flight.FlightDate, flight.Price, command.Description, emptySeat?.SeatNumber));

        await _flightGrpcService.ReserveSeat(new ReserveSeatRequestDto
        {
            FlightId = flight.Id, SeatNumber = emptySeat?.SeatNumber
        });

        var result = await _eventStoreDbRepository.Add(
            aggrigate,
            cancellationToken);

        return result;
    }
}
