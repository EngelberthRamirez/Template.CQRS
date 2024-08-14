using ApplicationCore.Domain;
using ApplicationCore.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationCore.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IPublisher publisher,
        ILogger<ApplicationDbContext> logger) : DbContext(options)
    {
        public DbSet<Product> Productos => Set<Product>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Ignore(x => x.DomainEvents);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

            foreach (var @event in events)
            {
                @event.IsPublished = true;

                logger.LogInformation("New domain event {Event}", @event.GetType().Name);

                await publisher.Publish(@event);
            }

            return result;
        }
    }
}
