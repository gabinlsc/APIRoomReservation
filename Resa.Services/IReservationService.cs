using Resa.Dto;

namespace Resa.Services;

public interface IReservationService
{
    public Task<IReadOnlyList<ReservationDtos>> GetAllAsync(CancellationToken ct = default);

    public Task<ReservationDtos?> GetByIdAsync(int id, CancellationToken ct = default);

    public Task<ReservationDtos> CreateAsync(ReservationDtos reservation, CancellationToken ct = default);

    public Task<ReservationDtos?> UpdateAsync(int id, ReservationDtos reservation, CancellationToken ct = default);

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
