using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampAdmin.API.Models
{
    public class TaschengeldBuchung
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TeilnehmerId { get; set; }
        public decimal Betrag { get; set; }
        public string Beschreibung { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }//ref Context
    }
}
