using System.Data;
using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Exceptions;
using ApplicationCore.Domain;
using Dapper;
using FluentValidation;
using MediatR;
using PJENL.Template.CQRS.ApplicationCore.Common.Abstractions.Messaging;

namespace ApplicationCore.Features.Products.Commands;

public class UpdateProductCommand : IRequest, ICacheInvalidationCommand
{
    public int Id { get; set; }
    public required UpdateProductCommandParameters Parameters { get; set; }

    public IEnumerable<string> CacheKeys => ["products", $"product-by-id-{Id}"];
}

public class UpdateProductCommandParameters
{
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UpdateProductCommandHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection("defaultConnection");
        var product = await connection.QuerySingleAsync<Product>("ObtenerProductoPorId", new { request.Id }, commandType: CommandType.StoredProcedure);

        if (product is null)
        {
            throw new NotFoundException();
        }

        product.Id = request.Id;
        product.Nombre = request.Parameters.Nombre;
        product.Precio = request.Parameters.Precio;
        product.Stock = request.Parameters.Stock;

        await connection.ExecuteAsync("ActualizarProducto", product, commandType: CommandType.StoredProcedure);
    }
}

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
        RuleFor(r => r.Parameters.Nombre).NotNull().NotEmpty();
        RuleFor(r => r.Parameters.Precio).GreaterThan(0).NotNull();
        RuleFor(r => r.Parameters.Stock).GreaterThan(0).NotNull();
    }
}
