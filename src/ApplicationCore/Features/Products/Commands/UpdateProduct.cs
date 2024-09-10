using System.Data;
using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Abstractions.Messaging;
using ApplicationCore.Common.Exceptions;
using ApplicationCore.Domain.Entities;
using Dapper;
using FluentValidation;
using MediatR;

namespace ApplicationCore.Features.Products.Commands;

public class UpdateProduct
{
    public class Request
    {
        public int Id { get; set; }
        public required Payload Payload { get; set; }
    }

    public class Payload
    {
        public string Nombre { get; set; } = default!;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Id).NotNull().NotEmpty();
            RuleFor(r => r.Payload.Nombre).NotNull().NotEmpty();
            RuleFor(r => r.Payload.Precio).GreaterThan(0).NotNull();
            RuleFor(r => r.Payload.Stock).GreaterThan(0).NotNull();
        }
    }

    public class Command : IRequest, ICacheInvalidationCommand
    {
        public Request Request { get; set; } = default!;
        public IEnumerable<string> CacheKeys => ["products", $"product-by-id-{Request.Id}"];
    }

    public class Handler(IDbConnectionFactory dbConnectionFactory) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            using var connection = dbConnectionFactory.CreateConnection("defaultConnection");
            var product = await connection.QuerySingleOrDefaultAsync<Product>("ObtenerProductoPorId", new { command.Request.Id }, commandType: CommandType.StoredProcedure) ?? throw new NotFoundException();
            product.Id = command.Request.Id;
            product.Nombre = command.Request.Payload.Nombre;
            product.Precio = command.Request.Payload.Precio;
            product.Stock = command.Request.Payload.Stock;

            await connection.ExecuteAsync("ActualizarProducto", product, commandType: CommandType.StoredProcedure);
        }
    }
}
