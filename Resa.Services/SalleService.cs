using Microsoft.EntityFrameworkCore;
using Resa.DataAccess;
using Resa.Dto;
using Resa.Entities;

namespace Resa.Services;

public class SalleService : ISalleService
{
    private readonly ReservationDbContext _db;
    public SalleService(ReservationDbContext db) => _db = db;

    private static SalleDtos ToDto(Salle s) => new()
    {
        Id = s.Id,
        Nom = s.Nom,
        Capacite = s.Capacite
    };

    public async Task<IReadOnlyList<SalleDtos>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Salles
            .AsNoTracking()
            .OrderBy(s => s.Nom)
            .Select(s => new SalleDtos { Id = s.Id, Nom = s.Nom, Capacite = s.Capacite })
            .ToListAsync(ct);
    }

    public async Task<SalleDtos?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var salle = await _db.Salles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        return salle is null ? null : ToDto(salle);
    }

    public async Task<SalleDtos> CreateAsync(SalleDtos dto, CancellationToken ct = default)
    {
        var salle = new Salle { Nom = dto.Nom, Capacite = dto.Capacite };
        _db.Salles.Add(salle);
        await _db.SaveChangesAsync(ct);
        return ToDto(salle);
    }

    public async Task<SalleDtos?> UpdateAsync(int id, SalleDtos dto, CancellationToken ct = default)
    {
        var salle = await _db.Salles.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (salle is null) return null;
        salle.Nom = dto.Nom;
        salle.Capacite = dto.Capacite;
        await _db.SaveChangesAsync(ct);
        return ToDto(salle);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var salle = await _db.Salles.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (salle is null) return false;
        if (await _db.Reservations.AnyAsync(r => r.SalleId == id, ct))
            throw new InvalidOperationException("Impossible de supprimer une salle qui a des réservations.");
        _db.Salles.Remove(salle);
        await _db.SaveChangesAsync(ct);
        return true;
    }

}
