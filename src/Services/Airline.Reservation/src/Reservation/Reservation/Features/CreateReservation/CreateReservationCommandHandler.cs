using Ardalis.GuardClauses;
using BuildingBlocks.Domain;
using BuildingBlocks.Grpc.Contracts;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Refit;
using Reservation.Configuration;
using Reservation.Data;
using Reservation.Reservation.Dtos;
using Reservation.Reservation.Exceptions;
using Reservation.Reservation.Models.ValueObjects;

namespace Reservation.Reservation.Features.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationResponseDto>
{
    private readonly ReservationDbContext _reservationDbContext;
    private readonly IMapper _mapper;
    private readonly IFlightGrpcService _flightGrpcService;
    private readonly IPassengerGrpcService _passengerGrpcService;


    public CreateReservationCommandHandler(
        IMapper mapper,
        ReservationDbContext reservationDbContext,
        IOptions<GrpcOptions> grpcOptions)
    {
        _mapper = mapper;
        _reservationDbContext = reservationDbContext;

        var channelFlight = GrpcChannel.ForAddress(grpcOptions.Value.FlightAddress);
        _flightGrpcService =
            new Lazy<IFlightGrpcService>(() => MagicOnionClient.Create<IFlightGrpcService>(channelFlight)).Value;

        var channelPassenger = GrpcChannel.ForAddress(grpcOptions.Value.PassengerAddress);
        _passengerGrpcService =
            new Lazy<IPassengerGrpcService>(() => MagicOnionClient.Create<IPassengerGrpcService>(channelPassenger))
                .Value;
    }

    public async Task<ReservationResponseDto> Handle(CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        try
        {
            var flight = await _flightGrpcService.GetById(command.FlightId);
            if (flight is null)
                throw new FlightNotFoundException();
            var passenger = await _passengerGrpcService.GetById(command.PassengerId);

            var emptySeat = (await _flightGrpcService.GetAvailableSeats(command.FlightId))?.First();

            var reservationEntity = Models.Reservation.Create(new PassengerInfo(passenger.Name), new Trip(
                flight.FlightNumber, flight.AircraftId, flight.DepartureAirportId,
                flight.ArriveAirportId, flight.FlightDate, flight.Price, command.Description, emptySeat?.SeatNumber));

            await _flightGrpcService.ReserveSeat(new ReserveSeatRequestDto
            {
                FlightId = flight.Id, SeatNumber = emptySeat?.SeatNumber
            });

            var newReservation =
                await _reservationDbContext.Reservations.AddAsync(reservationEntity, cancellationToken);

            return _mapper.Map<ReservationResponseDto>(newReservation.Entity);
        }
        catch (RpcException ex)
        {
            throw new Exception(ex.Status.Detail);
        }
    }
}
