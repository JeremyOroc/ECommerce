using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]      //attribute helps create precise API documentation
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider productsProvider;

        public ProductsController(IProductsProvider productsProvider)
        {
            this.productsProvider = productsProvider;
        }

        /// <summary>
        /// Gets all the products in the database.
        /// </summary>
        /// <returns>an IActionResult.</returns>
        /// <response code="200">Returns the requested products.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await productsProvider.GetProductsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Products);
            }
            return NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Get product by the provided id.
        /// </summary>
        /// <param name="id">The product id to get.</param>
        /// <returns>an IActionResult.</returns>
        /// <response code="200">Returns the requested product.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await productsProvider.GetProductAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
