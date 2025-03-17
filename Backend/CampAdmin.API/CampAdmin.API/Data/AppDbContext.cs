using CampAdmin.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CampAdmin.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Teilnehmer> Teilnehmer { get; set; }
        public DbSet<TaschengeldBuchung> TaschengeldBuchungen { get; set; }
    }
}
