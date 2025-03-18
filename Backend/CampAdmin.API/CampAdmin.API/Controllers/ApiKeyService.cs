using CampAdmin.API.Data;
using CampAdmin.API.Models;

namespace CampAdmin.API.Controllers
{
    public class ApiKeyService
    {
        private readonly AppDbContext _context;

        public ApiKeyService(AppDbContext context)
        {
            _context = context;
        }

        public ApiKey GenerateApiKey(string Name, string Desc, List<string> permissions)
        {
            var apiKey = new ApiKey { Permissions = permissions, Description=Desc, Name=Name};
            _context.ApiKeys.Add(apiKey);
            _context.SaveChanges();
            return apiKey;
        }

        public bool ValidateApiKey(string key, string requiredPermission)
        {
            var apiKey = _context.ApiKeys.FirstOrDefault(k => k.Key == key);
            return apiKey != null && apiKey.Permissions.Contains(requiredPermission);
        }
    }

}
