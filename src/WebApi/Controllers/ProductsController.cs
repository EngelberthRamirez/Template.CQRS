using ApplicationCore.Features.Products.Commands;
using ApplicationCore.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Crea un producto nuevo usando dapper
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("dapper")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.Request request)
    {
        await mediator.Send(new CreateProduct.Command { Request = request });
        return Ok();
    }

    /// <summary>
    /// Crea un producto nuevo usando efc
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("efc")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductEFC.Request request)
    {
        await mediator.Send(new CreateProductEFC.Command { Request = request });
        return Ok();
    }

    /// <summary>
    /// Actualiza un producto
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProduct.Payload request)
    {
        var command = new UpdateProduct.Command
        {
            Request = new UpdateProduct.Request
            {
                Id = id,
                Payload = request
            }
        };

        await mediator.Send(command);
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
        var command = new DeleteProduct.Command
        {
            Request = new DeleteProduct.Request { Id = id }
        };

        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Consulta los productos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<GetProducts.Response>> GetProducts() => await mediator.Send(new GetProducts.Query());

    /// <summary>
    /// Consulta un producto por su ID usando Dapper
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("{id}/dapper")]
    public async Task<GetProductById.Response> GetProductById(string id) =>
        await mediator.Send(new GetProductById.Query { Id = id });

    /// <summary>
    /// Consulta un producto por su ID usando Entity Framework Core
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("{id}/efc")]
    public async Task<GetProductByIdEFCQueryResponse> GetProductByIdEFC(string id) =>
        await mediator.Send(new GetProductByIdEFCQuery { Id = id });
}
