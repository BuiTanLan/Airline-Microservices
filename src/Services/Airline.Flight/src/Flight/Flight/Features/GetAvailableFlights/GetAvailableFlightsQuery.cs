using System;
using System.Collections.Generic;
using BuildingBlocks.Caching;
using Flight.Flight.Dtos;
using MediatR;

namespace Flight.Flight.Features.GetAvailableFlights;

public record GetAvailableFlightsQuery : IRequest<IEnumerable<FlightResponseDto>>, ICacheRequest
{
    public string CacheKey { set; get; } = "GetAvailableFlightsQuery";
    public DateTime? AbsoluteExpirationRelativeToNow { set; get; } = DateTime.Now.AddHours(1);
}
