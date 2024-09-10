using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Abstractions.Messaging;
using Dapper;
using FluentValidation;
using MediatR;

namespace ApplicationCore.Features.Products.Commands;

public class CreateProduct
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

    public class Handler(IDbConnectionFactory dbConnectionFactory) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            using var connection = dbConnectionFactory.CreateConnection("defaultConnection");
            await connection.ExecuteAsync("InsertarProducto", request, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
