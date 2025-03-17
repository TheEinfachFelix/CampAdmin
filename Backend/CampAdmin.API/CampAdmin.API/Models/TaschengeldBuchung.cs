namespace CampAdmin.API.Models
{
    public class TaschengeldBuchung
    {
        public int Id { get; set; }
        public int TeilnehmerId { get; set; }
        public decimal Betrag { get; set; }
        public string Beschreibung { get; set; } = string.Empty;
        public DateTime Datum { get; set; } = DateTime.UtcNow;
    }
}
