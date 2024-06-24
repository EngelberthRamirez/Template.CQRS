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

public class GetProductByIdQuery : ICachedQuery<GetProductByIdQueryResponse>
{
    public string Id { get; set; }

    public string Key => $"product-by-id-{Id}";

    public TimeSpan? Expiration => null;
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetProductByIdQueryHandler(IDbConnectionFactory dbConnectionFactory, IMapper mapper)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _mapper = mapper;
    }
    public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {

        using IDbConnection connection = _dbConnectionFactory.CreateConnection("defaultConnection");
        var parameters = new { Id = request.Id.FromHashId() };
        var product = await connection.QuerySingleAsync<Product>("ObtenerProductoPorId", parameters, commandType: CommandType.StoredProcedure);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        return _mapper.Map<GetProductByIdQueryResponse>(product);
    }
}

public class GetProductByIdQueryResponse
{
    public string ProductoId { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class GetProductByIdQueryProfile : Profile
{
    public GetProductByIdQueryProfile() =>
        CreateMap<Product, GetProductByIdQueryResponse>()
            .ForMember(dest =>
                dest.ProductoId,
                opt => opt.MapFrom(mf => mf.Id.ToHashId()));
}