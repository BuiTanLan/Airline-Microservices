using System.Collections.Generic;
using BuildingBlocks.Caching;
using Flight.Flight.Dtos;
using MediatR;

namespace Flight.Flight.Features.GetAvailableFlights;

public record GetAvailableFlightsQuery : IRequest<IEnumerable<FlightResponseDto>>,
    ICacheRequest<GetAvailableFlightsQuery, IEnumerable<FlightResponseDto>>;
