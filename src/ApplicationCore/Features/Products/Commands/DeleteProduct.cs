using System.Data;
using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Abstractions.Messaging;
using Dapper;
using MediatR;

namespace ApplicationCore.Features.Products.Commands;

public class DeleteProduct
{
    public class Request
    {
        public int Id { get; set; }
    }

    public class Command : IRequest, ICacheInvalidationCommand
    {
        public Request Request { get; set; } = default!;
        public IEnumerable<string> CacheKeys => ["products"];
    }

    public class Handler(IDbConnectionFactory dbConnectionFactory) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            using var connection = dbConnectionFactory.CreateConnection("defaultConnection");
            await connection.ExecuteAsync("EliminarProductoPorId", new { command.Request.Id }, commandType: CommandType.StoredProcedure);
        }
    }
}
