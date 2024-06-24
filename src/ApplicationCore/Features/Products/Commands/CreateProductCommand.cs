using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Abstractions.Messaging;
using Dapper;
using FluentValidation;
using MediatR;

namespace ApplicationCore.Features.Products.Commands;

public class CreateProductCommand : IRequest, ICacheInvalidationCommand
{
    public required CreateProductCommandParameters Parameters { get; set; }

    public IEnumerable<string> CacheKeys => ["products"];
}

public class CreateProductCommandParameters
{
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CreateProductCommandHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection("defaultConnection");
        await connection.ExecuteAsync("InsertarProducto", request.Parameters, commandType: System.Data.CommandType.StoredProcedure);
    }
}

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(r => r.Parameters.Nombre).NotEmpty();
        RuleFor(r => r.Parameters.Precio).NotNull().GreaterThan(0);
        RuleFor(r => r.Parameters.Stock).NotNull().GreaterThan(0);
    }
}
