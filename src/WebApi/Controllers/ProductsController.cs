using ApplicationCore.Features.Products.Commands;
using ApplicationCore.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crea un producto nuevo usando dapper
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("dapper")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommandParameters command)
    {
        await _mediator.Send(new CreateProductCommand { Parameters = command });
        return Ok();
    }

    /// <summary>
    /// Crea un producto nuevo usando efc
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("efc")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductEFCCommandParameters command)
    {
        await _mediator.Send(new CreateProductEFCCommand { Parameters = command });
        return Ok();
    }

    /// <summary>
    /// Actualiza un producto
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductCommandParameters command)
    {
        await _mediator.Send(new UpdateProductCommand { Id = id, Parameters = command });
        return NoContent();
    }

    /// <summary>
    /// Borra un producto
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Consulta los productos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<GetProductsQueryResponse>> GetProducts() => await _mediator.Send(new GetProductsQuery());

    /// <summary>
    /// Consulta un producto por su ID usando Dapper
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("{id}/dapper")]
    public async Task<GetProductByIdQueryResponse> GetProductById(string id) =>
        await _mediator.Send(new GetProductByIdQuery { Id = id });

    /// <summary>
    /// Consulta un producto por su ID usando Entity Framework Core
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("{id}/efc")]
    public async Task<GetProductByIdEFCQueryResponse> GetProductByIdEFC(string id) =>
        await _mediator.Send(new GetProductByIdEFCQuery { Id = id });
}
