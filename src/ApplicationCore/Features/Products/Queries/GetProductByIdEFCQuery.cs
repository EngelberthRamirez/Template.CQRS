using ApplicationCore.Common.Exceptions;
using ApplicationCore.Domain.Entities;
using ApplicationCore.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using MediatrExample.ApplicationCore.Common.Helpers;

namespace ApplicationCore.Features.Products.Queries;

public class GetProductByIdEFCQuery : IRequest<GetProductByIdEFCQueryResponse>
{
    public string Id { get; set; }
}

public class GetProductByIdEFCQueryHandler : IRequestHandler<GetProductByIdEFCQuery, GetProductByIdEFCQueryResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProductByIdEFCQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetProductByIdEFCQueryResponse> Handle(GetProductByIdEFCQuery request, CancellationToken cancellationToken)
    {

        var product = await _context.Productos.FindAsync(request.Id.FromHashId());

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        return _mapper.Map<GetProductByIdEFCQueryResponse>(product);
    }
}

public class GetProductByIdEFCQueryResponse
{
    public string ProductoId { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class GetProductByIdEFCQueryProfile : Profile
{
    public GetProductByIdEFCQueryProfile() =>
        CreateMap<Product, GetProductByIdEFCQueryResponse>()
            .ForMember(dest =>
                dest.ProductoId,
                opt => opt.MapFrom(mf => mf.Id.ToHashId()));
}