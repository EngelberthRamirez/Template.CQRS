using System.Data;
using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Abstractions.Messaging;
using ApplicationCore.Domain.Entities;
using AutoMapper;
using Dapper;
using MediatR;
using MediatrExample.ApplicationCore.Common.Helpers;

namespace ApplicationCore.Features.Products.Queries;

public class GetProducts
{
    public class Response
    {
        public string ProductoId { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public decimal Precio { get; set; }
        public string Descripcion { get; set; } = default!;
    }

    public class GetProductsProfile : Profile
    {
        public GetProductsProfile() =>
            CreateMap<Product, Response>()
                .ForMember(dest =>
                    dest.Descripcion,
                    opt => opt.MapFrom(mf => $"{mf.Nombre} - {mf.Precio:c}"))
                .ForMember(dest =>
                        dest.ProductoId,
                        opt => opt.MapFrom(mf => mf.Id.ToHashId()));
    }

    public class Query : ICachedQuery<List<Response>>
    {
        public string Key => "products";
        public TimeSpan? Expiration => null;
    }

    public class Handler(IDbConnectionFactory dbConnectionFactory, IMapper mapper) : IRequestHandler<Query, List<Response>>
    {
        public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection("defaultConnection");
            var products = await connection.QueryAsync<Product>("ObtenerProductos", commandType: CommandType.StoredProcedure);
            return mapper.Map<List<Response>>(products);
        }
    }
}
