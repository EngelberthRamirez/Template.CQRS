using ApplicationCore.Common.Abstractions.Messaging;
using ApplicationCore.Domain.Entities;
using ApplicationCore.Infrastructure.Persistence.Context;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace ApplicationCore.Features.Products.Commands;

public class CreateProductEFCCommand : IRequest, ICacheInvalidationCommand
{
    public required CreateProductEFCCommandParameters Parameters { get; set; }

    public IEnumerable<string> CacheKeys => ["products"];
}

public class CreateProductEFCCommandParameters
{
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class CreateProductEFCCommandHandler : IRequestHandler<CreateProductEFCCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProductEFCCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(CreateProductEFCCommand request, CancellationToken cancellationToken)
    {
        var producto = Product.Create(0, request.Parameters.Nombre, request.Parameters.Precio, request.Parameters.Stock);
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class CreateProductEFCValidator : AbstractValidator<CreateProductEFCCommand>
{
    public CreateProductEFCValidator()
    {
        RuleFor(r => r.Parameters.Nombre).NotEmpty();
        RuleFor(r => r.Parameters.Precio).NotNull().GreaterThan(0);
        RuleFor(r => r.Parameters.Stock).NotNull().GreaterThan(0);
    }
}
