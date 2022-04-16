using System.Reflection;
using BuildingBlocks.EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Reservation.Data;

public class ReservationDbContext : AppDbContextBase
{
    public ReservationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
    {
    }

    public DbSet<Reservations.Models.Reservation> Reservations => Set<Reservations.Models.Reservation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
