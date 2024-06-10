using Microsoft.EntityFrameworkCore;
using PJENL.API.CleanArchitecture.ApplicationCore.Domain;

namespace ApplicationCore.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Productos => Set<Product>();
    }
}
