using Microsoft.EntityFrameworkCore;
using TechStore.Api.Models;

namespace TechStore.Api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
            
        }

        public DbSet<Produs> Produse { get; set; }
        public DbSet<Comanda> Comenzi { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
