using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_App_Sql.Models;

namespace CRUD_App_Sql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Master2Context _context;

        public ProductsController(Master2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirements
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }

        [HttpGet("descending")]
        public async Task<IActionResult> GetDescending()
        {
            try
            {
                var products = await _context.Products.OrderByDescending(p => p.Id).ToListAsync();
                if (products.Count == 0)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirements
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products in descending order.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirements
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the product.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirements
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Product productData)
        {
            try
            {
                if (productData == null || productData.Id == 0)
                    return BadRequest();

                var product = await _context.Products.FindAsync(productData.Id);
                if (product == null)
                    return NotFound();

                product.Name = productData.Name;
                product.Description = productData.Description;
                product.Price = productData.Price;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirements
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirements
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }
        }
    }
}
