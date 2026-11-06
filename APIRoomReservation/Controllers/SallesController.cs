using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resa.Dto;
using Resa.Services;

namespace APIRoomReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SallesController : ControllerBase
    {
        private readonly ISalleService _salleService;

        public SallesController(ISalleService salleService)
        {
            _salleService = salleService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SalleDtos>>> GetAllAsync(CancellationToken ct)
        {
            var salles = await _salleService.GetAllAsync(ct);
            return Ok(salles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalleDtos?>> GetByIdAsync(int id, CancellationToken ct)
        {
            var salle = await _salleService.GetByIdAsync(id, ct);
            if (salle == null)
                return NotFound();
            return Ok(salle);
        }

        [HttpPost]
        public async Task<ActionResult<SalleDtos>> CreateAsync(SalleDtos salle, CancellationToken ct)
        {
            var createdSalle = await _salleService.CreateAsync(salle, ct);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdSalle.Id }, createdSalle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SalleDtos>> UpdateAsync(int id, SalleDtos salle, CancellationToken ct)
        {
            var updatedSalle = await _salleService.UpdateAsync(id, salle, ct);
            if (updatedSalle == null)
                return NotFound();
            return Ok(updatedSalle);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            try
            {
                var deleted = await _salleService.DeleteAsync(id, ct);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
