using System.Net;
using LearnProject.Dtos.request;
using LearnProject.Dtos.response;
using LearnProject.Models;
using LearnProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<ProductModel>>>> GetAllProducts()
        {
            BaseResponse<IEnumerable<ProductModel>> response = new()
            {
                Message = "Get all products successfully",
                Data = await _productService.GetAllProducts(),
                StatusCode = (int)HttpStatusCode.OK
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return Ok(product);

            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new { message = ex.Message, StatusCode = 400 }); 
            }

            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, StatusCode = 400 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }

        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct(ProductRequest productRequest)
        {
            try
            {
                var newProduct = await _productService.CreateProduct(productRequest);
                return Ok(newProduct);
            }

            catch(KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message, StatusCode = 400 });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> UpdateProduct(int id, ProductRequest productRequest)
        {
            try
            {
                var product = await _productService.UpdateProductById(id, productRequest);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Internal Server Error", ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductById(id);
                return Ok(new { message = $"delete product {id} successfully", StatusCode = HttpStatusCode.OK });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Internal Server Error", ex.Message });
            }
        }



    }
}
