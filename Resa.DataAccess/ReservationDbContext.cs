using Microsoft.EntityFrameworkCore;
using Resa.Entities;
using System.Reflection.Emit;

namespace Resa.DataAccess;

public class ReservationDbContext : DbContext
{
    public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Salle> Salles => Set<Salle>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(reservation => reservation.User)
            .WithMany(user => user.Reservations)
            .HasForeignKey(reservation => reservation.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(reservation => reservation.Salle)
            .WithMany(salle => salle.Reservations)
            .HasForeignKey(reservation => reservation.SalleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}