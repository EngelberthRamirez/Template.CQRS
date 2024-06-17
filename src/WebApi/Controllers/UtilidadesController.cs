using MediatrExample.ApplicationCore.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class UtilidadesController : ControllerBase
    {
        /// <summary>
        /// Genera un hash para un número entero dado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("generate-hash/{id}")]
        public IActionResult GenerateHash(int id)
        {
            string hash = id.ToHashId();
            return Ok(new { Hash = hash });
        }
    }
}
