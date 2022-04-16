using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.EventStoreDB.Repository;
using BuildingBlocks.Persistence;
using Flight.Aircrafts.Models;
using Flight.Airports.Models;
using Flight.Flights.Models;
using Flight.Seats.Models;

namespace Flight;

public class FlightDataSeeder : IDataSeeder
{
    private readonly IEventStoreDBRepository<Airport> _airportRepository;
    private readonly IEventStoreDBRepository<Aircraft> _aircraftRepository;
    private readonly IEventStoreDBRepository<Seat> _seatRepository;
    private readonly IEventStoreDBRepository<Flights.Models.Flight> _flightRepository;

    public FlightDataSeeder(IEventStoreDBRepository<Airport> airportRepository,
        IEventStoreDBRepository<Aircraft> aircraftRepository,
        IEventStoreDBRepository<Seat> seatRepository,
        IEventStoreDBRepository<Flights.Models.Flight> flightRepository)
    {
        _seatRepository = seatRepository;
        _flightRepository = flightRepository;
        _airportRepository = airportRepository;
        _aircraftRepository = aircraftRepository;
    }

    public async Task SeedAllAsync()
    {
        await SeedAirportAsync();
        await SeedAircraftAsync();
        await SeedFlightAsync();
        await SeedSeatAsync();
    }

    private async Task SeedAirportAsync()
    {
        if (await _airportRepository.Find(2399187385188352, CancellationToken.None) == null)
        {
            await _airportRepository.Add(
                Airport.Create(2399187385188352, "Lisbon International Airport", "LIS", "12988"), CancellationToken.None);

            await _airportRepository.Add(
                Airport.Create(2499187385188353, "Tehran International Airport", "Teh", "18000"), CancellationToken.None);
        }
    }


    private async Task SeedFlightAsync()
    {
        if (await _flightRepository.Find(7799187385188353, CancellationToken.None) == null)
        {
            await _flightRepository.Add(
                Flights.Models.Flight.Create(7799187385188353, "12CC", 1399187385188352, 2399187385188352, new DateTime(2022,6,1, 10, 0 , 0),
                    new DateTime(2022,6,1, 14, 0, 0), 2499187385188353, 200, new DateTime(2022,6,1),
                    FlightStatus.Completed, 8000), CancellationToken.None);
        }
    }

    private async Task SeedAircraftAsync()
    {
        if (await _aircraftRepository.Find(1399187385188352, CancellationToken.None) == null)
        {
            await _aircraftRepository.Add(
                Aircraft.Create(1399187385188352, "Boeing 737", "B737", 2005), CancellationToken.None);
        }
    }

    private async Task SeedSeatAsync()
    {
        if (await _seatRepository.Find(4399187385188352, CancellationToken.None) == null)
        {
            await _seatRepository.Add(
                Seat.Create(4399187385188352, "12A", SeatType.Window, SeatClass.Economy, 7799187385188353), CancellationToken.None);

            await _seatRepository.Add(
                Seat.Create(4499187385188352, "12B", SeatType.Window, SeatClass.Economy, 7799187385188353), CancellationToken.None);

            await _seatRepository.Add(
                Seat.Create(4599187385188352, "12C", SeatType.Middle, SeatClass.Economy, 7799187385188353), CancellationToken.None);

            await _seatRepository.Add(
                Seat.Create(4699187385188352, "12D", SeatType.Middle, SeatClass.Economy, 7799187385188353), CancellationToken.None);

            await _seatRepository.Add(
                Seat.Create(4799187385188352, "12F", SeatType.Aisle, SeatClass.Economy, 7799187385188353), CancellationToken.None);
        }
    }
}
