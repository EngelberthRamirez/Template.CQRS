using System.Data;
using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Abstractions.Messaging;
using ApplicationCore.Common.Exceptions;
using ApplicationCore.Domain.Entities;
using AutoMapper;
using Dapper;
using MediatR;
using MediatrExample.ApplicationCore.Common.Helpers;

namespace ApplicationCore.Features.Products.Queries;

public class GetProductById
{
    public class Response
    {
        public string ProductoId { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class GetProductByIdProfile : Profile
    {
        public GetProductByIdProfile() =>
            CreateMap<Product, Response>()
                .ForMember(dest =>
                    dest.ProductoId,
                    opt => opt.MapFrom(mf => mf.Id.ToHashId()));
    }

    public class Query : ICachedQuery<Response>
    {
        public string Id { get; set; }
        public string Key => $"product-by-id-{Id}";
        public TimeSpan? Expiration => null;
    }

    public class Handler(IDbConnectionFactory dbConnectionFactory, IMapper mapper) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection("defaultConnection");
            var parameters = new { Id = request.Id.FromHashId() };
            var product = await connection.QuerySingleAsync<Product>("ObtenerProductoPorId", parameters, commandType: CommandType.StoredProcedure);

            return product is null ? throw new NotFoundException(nameof(Product), request.Id) : mapper.Map<Response>(product);
        }
    }
}
