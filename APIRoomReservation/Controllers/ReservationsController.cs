using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resa.Dto;
using Resa.Services;

namespace APIRoomReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReservationDtos>>> GetAllAsync(CancellationToken ct)
        {
            var reservations = await _reservationService.GetAllAsync(ct);
            return Ok(reservations);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ReservationDtos?>> GetByIdAsync(int id, CancellationToken ct)
        {
            var reservation = await _reservationService.GetByIdAsync(id, ct);
            if (reservation == null)
                return NotFound();
            return Ok(reservation);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ReservationDtos>> CreateAsync(ReservationDtos reservation, CancellationToken ct)
        {
            var createdReservation = await _reservationService.CreateAsync(reservation, ct);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdReservation.Id }, createdReservation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationDtos?>> UpdateAsync(int id, ReservationDtos reservation, CancellationToken ct)
        {
            var updatedReservation = await _reservationService.UpdateAsync(id, reservation, ct);
            if (updatedReservation == null)
                return NotFound();
            return Ok(updatedReservation);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            var deleted = await _reservationService.DeleteAsync(id, ct);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

    }
}
