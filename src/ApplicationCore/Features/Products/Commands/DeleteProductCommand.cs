using Dapper;
using MediatR;
using PJENL.API.CleanArchitecture.ApplicationCore.Common.Abstractions.Data;
using PJENL.Template.CQRS.ApplicationCore.Common.Abstractions.Messaging;
using System.Data;

namespace MediatrExample.ApplicationCore.Features.Products.Commands;

public class DeleteProductCommand : IRequest, ICacheInvalidationCommand
{
    public int Id { get; set; }

    public IEnumerable<string> CacheKeys => ["products"];
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DeleteProductCommandHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection("defaultConnection");
        await connection.ExecuteAsync("EliminarProductoPorId", new { request.Id }, commandType: CommandType.StoredProcedure);
    }
}
