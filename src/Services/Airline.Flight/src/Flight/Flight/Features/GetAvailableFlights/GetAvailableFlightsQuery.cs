using System;
using System.Collections.Generic;
using BuildingBlocks.Caching;
using Flight.Flight.Dtos;
using MediatR;

namespace Flight.Flight.Features.GetAvailableFlights;

public record GetAvailableFlightsQuery : IRequest<IEnumerable<FlightResponseDto>>,
    ICacheRequest<GetAvailableFlightsQuery, IEnumerable<FlightResponseDto>>
{
    public string CacheKey => "GetAvailableFlightsQuery";
    public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddHours(1);
}
