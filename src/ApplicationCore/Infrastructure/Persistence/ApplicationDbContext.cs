using ApplicationCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Productos => Set<Product>();
    }
}
