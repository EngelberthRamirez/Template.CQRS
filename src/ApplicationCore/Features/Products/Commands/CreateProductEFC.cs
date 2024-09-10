using ApplicationCore.Common.Abstractions.Messaging;
using ApplicationCore.Domain.Entities;
using ApplicationCore.Infrastructure.Persistence.Context;
using FluentValidation;
using MediatR;

namespace ApplicationCore.Features.Products.Commands;

public class CreateProductEFC
{
    public class Request
    {
        public string Nombre { get; set; } = default!;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Nombre).NotEmpty();
            RuleFor(r => r.Precio).NotNull().GreaterThan(0);
            RuleFor(r => r.Stock).NotNull().GreaterThan(0);
        }
    }

    public class Command : IRequest, ICacheInvalidationCommand
    {
        public required Request Request { get; set; }
        public IEnumerable<string> CacheKeys => ["products"];
    }

    public class Handler(ApplicationDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var producto = Product.Create(0, command.Request.Nombre, command.Request.Precio, command.Request.Stock);
            context.Productos.Add(producto);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
