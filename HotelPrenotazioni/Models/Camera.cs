namespace HotelPrenotazioni.Models;

public class Camera
{
    public int CameraId { get; set; }
    public int Numero { get; set; }
    public string Tipo { get; set; }
    public decimal Prezzo { get; set; }

    public ICollection<Prenotazione>? Prenotazioni { get; set; }
}
