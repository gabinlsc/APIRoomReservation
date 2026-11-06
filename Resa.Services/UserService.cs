using Microsoft.EntityFrameworkCore;
using Resa.DataAccess;
using Resa.Dto;
using Resa.Entities;

namespace Resa.Services;

public class UserService : IUserService
{
    private readonly ReservationDbContext _db;

    public UserService(ReservationDbContext db) => _db = db;

    private static UserDtos ToDto(User u) => new()
    {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName
    };

    public async Task<IReadOnlyList<UserDtos>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Users
            .AsNoTracking()
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .Select(u => new UserDtos { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName })
            .ToListAsync(ct);
    }

    public async Task<UserDtos?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
        return user is null ? null : ToDto(user);
    }

    public async Task<UserDtos> CreateAsync(UserDtos dto, CancellationToken ct = default)
    {
        var user = new User { FirstName = dto.FirstName, LastName = dto.LastName };
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);   
        return ToDto(user);
    }

    public async Task<UserDtos?> UpdateAsync(int id, UserDtos dto, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return null;    

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        await _db.SaveChangesAsync(ct);
        return ToDto(user);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return false;   

        if (await _db.Reservations.AnyAsync(r => r.UserId == id, ct))
            throw new InvalidOperationException("Impossible de supprimer un utilisateur qui a des réservations.");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
