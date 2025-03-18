namespace CampAdmin.API.Models
{
    public class ApiKey
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; } = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        public List<string> Permissions { get; set; } = new List<string>();
        public string Description { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
