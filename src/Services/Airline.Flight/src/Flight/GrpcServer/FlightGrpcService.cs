using System.Collections.Generic;
using BuildingBlocks.Grpc.Contracts;
using Flight.Flight.Features.GetFlightById;
using Flight.Seat.Features.GetAvailableSeats;
using Flight.Seat.Features.ReserveSeat;
using MagicOnion;
using MagicOnion.Server;
using Mapster;
using MediatR;
using SeatResponseDto = BuildingBlocks.Grpc.Contracts.SeatResponseDto;

namespace Flight.GrpcServer;

public class FlightGrpcService : ServiceBase<IFlightGrpcService>, IFlightGrpcService
{
    private readonly IMediator _mediator;

    public FlightGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<FlightResponseDto> GetById(long id)
    {
        var result = await _mediator.Send(new GetFlightByIdQuery(id));
        return result.Adapt<FlightResponseDto>();
    }

    public async UnaryResult<IEnumerable<SeatResponseDto>> GetAvailableSeats(long flightId)
    {
        var result = await _mediator.Send(new GetAvailableSeatsQuery(flightId));
        return result.Adapt<IEnumerable<SeatResponseDto>>();
    }

    public async UnaryResult<FlightResponseDto> ReserveSeat(ReserveSeatRequestDto request)
    {
        var result = await _mediator.Send(new ReserveSeatCommand(request.FlightId, request.SeatNumber));
        return result.Adapt<FlightResponseDto>();
    }
}
