using Resa.Dto;

namespace Resa.Services;

public interface ISalleService
{
    Task<IReadOnlyList<SalleDtos>> GetAllAsync(CancellationToken ct = default);
    Task<SalleDtos?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<SalleDtos> CreateAsync(SalleDtos salle, CancellationToken ct = default);
    Task<SalleDtos?> UpdateAsync(int id, SalleDtos salle, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);

}
