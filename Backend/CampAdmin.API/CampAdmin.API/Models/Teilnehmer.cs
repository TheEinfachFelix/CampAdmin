namespace CampAdmin.API.Models
{
    public class Teilnehmer
    {
        public int Id { get; set; }
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public DateOnly Geburtsdatum { get; set; }
        public decimal Taschengeld { get; set; }
    }
}
