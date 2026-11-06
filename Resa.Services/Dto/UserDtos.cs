using Resa.DataAccess.Entities;

namespace Resa.Services.Dto;

public class UserDtos
{
    public int id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Reservations> Reservations { get; set; } = new List<Reservations>();
}
