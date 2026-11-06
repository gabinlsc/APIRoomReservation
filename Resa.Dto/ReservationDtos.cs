namespace Resa.Dto;

/// <summary>Réservation d'une salle sur un créneau (lecture / création / modification).</summary>
public class ReservationDtos
{
    public int Id { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public int NombrePersonnes { get; set; }
    public int SalleId { get; set; }
    public int UserId { get; set; }
}
