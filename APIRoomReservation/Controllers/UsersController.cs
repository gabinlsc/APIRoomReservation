using Microsoft.AspNetCore.Mvc;
using Resa.Dto;
using Resa.Services;

namespace APIRoomReservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserDtos>>> GetAllAsync(CancellationToken ct)
        {
            var users = await _userService.GetAllAsync(ct);
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDtos?>> GetByIdAsync(int id, CancellationToken ct)
        {
            var user = await _userService.GetByIdAsync(id, ct);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDtos>> CreateAsync(UserDtos user, CancellationToken ct)
        {
            var createdUser = await _userService.CreateAsync(user, ct);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdUser.Id }, createdUser);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDtos?>> UpdateAsync(int id, UserDtos user, CancellationToken ct)
        {
            var updatedUser = await _userService.UpdateAsync(id, user, ct);
            if (updatedUser == null)
                return NotFound();
            return Ok(updatedUser);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            var deleted = await _userService.DeleteAsync(id, ct);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}