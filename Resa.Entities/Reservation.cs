using System.ComponentModel.DataAnnotations;

namespace Resa.Entities;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    public DateTime DateDebut { get; set; }

    [Required]
    public DateTime DateFin { get; set; }

    [Range(1, 1000)]
    public int NombrePersonnes { get; set; }

    public int SalleId { get; set; }
    public Salle Salle { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
