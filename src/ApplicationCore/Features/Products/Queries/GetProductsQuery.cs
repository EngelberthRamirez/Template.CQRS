using System.Data;
using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Domain;
using AutoMapper;
using Dapper;
using MediatR;
using MediatrExample.ApplicationCore.Common.Helpers;
using PJENL.Template.CQRS.ApplicationCore.Common.Abstractions.Messaging;

namespace ApplicationCore.Features.Products.Queries;

public class GetProductsQuery : ICachedQuery<List<GetProductsQueryResponse>>
{
    public string Key => "products";
    public TimeSpan? Expiration => null;
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductsQueryResponse>>
{
    private readonly IMapper _mapper;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetProductsQueryHandler(IDbConnectionFactory dbConnectionFactory, IMapper mapper)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _mapper = mapper;
    }

    public async Task<List<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection("defaultConnection");
        var products = await connection.QueryAsync<Product>("ObtenerProductos", commandType: CommandType.StoredProcedure);
        return _mapper.Map<List<GetProductsQueryResponse>>(products);
    }
}

public class GetProductsQueryResponse
{
    public string ProductoId { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public string Descripcion { get; set; } = default!;
}

public class GetProductsQueryProfile : Profile
{
    public GetProductsQueryProfile() =>
        CreateMap<Product, GetProductsQueryResponse>()
            .ForMember(dest =>
                dest.Descripcion,
                opt => opt.MapFrom(mf => $"{mf.Nombre} - {mf.Precio:c}"))
            .ForMember(dest =>
                    dest.ProductoId,
                    opt => opt.MapFrom(mf => mf.Id.ToHashId()));
}
