using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampAdmin.API.Models
{
    public class ApiKey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; } = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        public List<string> Permissions { get; set; } = new List<string>();
        public string Description { get; set; } = "";
        public string Name { get; set; } = "";
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } //ref Context
    }
}
