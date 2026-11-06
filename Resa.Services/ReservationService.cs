using Microsoft.EntityFrameworkCore;
using Resa.DataAccess;
using Resa.Dto;
using Resa.Entities;

namespace Resa.Services;

public class ReservationService : IReservationService
{
    private readonly ReservationDbContext _db;

    public ReservationService(ReservationDbContext db) => _db = db;

    private static ReservationDtos ToDto(Reservation r) => new()
    {
        Id = r.Id,
        UserId = r.UserId,
        SalleId = r.SalleId,
        DateDebut = r.DateDebut,
        DateFin = r.DateFin,
        NombrePersonnes = r.NombrePersonnes
    };

    private static DateTime ToUtc(DateTime d) => d.Kind switch
    {
        DateTimeKind.Utc => d,
        DateTimeKind.Local => d.ToUniversalTime(),
        _ => DateTime.SpecifyKind(d, DateTimeKind.Utc)
    };

    private async Task ValidateAsync(ReservationDtos dto, int? excludeId, CancellationToken ct)
    {
        if (dto.DateFin <= dto.DateDebut)
            throw new BusinessRuleException("La date de fin doit être après la date de début.");

        if (dto.NombrePersonnes < 1)
            throw new BusinessRuleException("Le nombre de personnes doit être d'au moins 1.");

        var salle = await _db.Salles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == dto.SalleId, ct)
            ?? throw new BusinessRuleException("Salle inexistante.");

        if (!await _db.Users.AnyAsync(u => u.Id == dto.UserId, ct))
            throw new BusinessRuleException("Utilisateur inexistant.");

        if (dto.NombrePersonnes > salle.Capacite)
            throw new BusinessRuleException($"La salle ne peut accueillir que {salle.Capacite} personnes.");
        var conflit = await _db.Reservations.AnyAsync(r =>
            r.SalleId == dto.SalleId &&
            r.Id != excludeId &&
            r.DateDebut < dto.DateFin &&
            r.DateFin > dto.DateDebut, ct);

        if (conflit)
            throw new ConflictException("Cette salle est déjà réservée sur ce créneau.");
    }

    public async Task<IReadOnlyList<ReservationDtos>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Reservations
            .AsNoTracking()
            .OrderBy(r => r.DateDebut)
            .Select(r => new ReservationDtos
            {
                Id = r.Id,
                UserId = r.UserId,
                SalleId = r.SalleId,
                DateDebut = r.DateDebut,
                DateFin = r.DateFin,
                NombrePersonnes = r.NombrePersonnes
            })
            .ToListAsync(ct);
    }

    public async Task<ReservationDtos?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var reservation = await _db.Reservations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
        return reservation is null ? null : ToDto(reservation);
    }

    public async Task<ReservationDtos> CreateAsync(ReservationDtos dto, CancellationToken ct = default)
    {
        dto.DateDebut = ToUtc(dto.DateDebut);
        dto.DateFin = ToUtc(dto.DateFin);
        await ValidateAsync(dto, excludeId: null, ct);

        var reservation = new Reservation
        {
            UserId = dto.UserId,
            SalleId = dto.SalleId,
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin,
            NombrePersonnes = dto.NombrePersonnes
        };
        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync(ct);
        return ToDto(reservation);
    }

    public async Task<ReservationDtos?> UpdateAsync(int id, ReservationDtos dto, CancellationToken ct = default)
    {
        var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (reservation is null) return null;   // -> 404

        dto.DateDebut = ToUtc(dto.DateDebut);
        dto.DateFin = ToUtc(dto.DateFin);
        await ValidateAsync(dto, excludeId: id, ct);

        reservation.UserId = dto.UserId;
        reservation.SalleId = dto.SalleId;
        reservation.DateDebut = dto.DateDebut;
        reservation.DateFin = dto.DateFin;
        reservation.NombrePersonnes = dto.NombrePersonnes;
        await _db.SaveChangesAsync(ct);
        return ToDto(reservation);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (reservation is null) return false;
        _db.Reservations.Remove(reservation);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
