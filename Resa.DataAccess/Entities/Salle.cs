using System.ComponentModel.DataAnnotations;

namespace Resa.Entities;

public class Salle
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Capacite { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}