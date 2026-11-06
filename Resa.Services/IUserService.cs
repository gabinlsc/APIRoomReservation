using Resa.Dto;

namespace Resa.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserDtos>> GetAllAsync(CancellationToken ct = default);

    Task<UserDtos?> GetByIdAsync(int id, CancellationToken ct = default);

    Task <UserDtos> CreateAsync(UserDtos user, CancellationToken ct = default);

    Task<UserDtos?> UpdateAsync(int id, UserDtos user, CancellationToken ct = default);

    Task<bool> DeleteAsync(int id, CancellationToken ct = default);


}
