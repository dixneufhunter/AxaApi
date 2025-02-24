using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SurveyApi.Models;
using SurveyApi.Data;
using Newtonsoft.Json;
using SurveyApi.DTO.Response;
using Microsoft.EntityFrameworkCore;

namespace SurveyApi.Controllers
{
    
    //[Route("[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly AppDbContext_Product _dbContext;

        public ProductController(AppDbContext_Product dbContext)
        {
            _dbContext = dbContext;
        }

        //private readonly IProduct _userService;
        //public UserController(IUserService userService)
        //{
        //    _userService = userService;
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            //var data = await _dbContext.Product.Include(c => c.ID).ToListAsync();
            var data = await _dbContext.Product.ToListAsync();

            return data;

        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product newProd)
        {
            if (newProd == null)
            {
                return BadRequest("Data tidak valid");
            }

            try
            {
                _dbContext.Product.Add(newProd);
                await _dbContext.SaveChangesAsync();
                return Ok(newProd);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Gagal menyimpan data : " + ex.Message);
            }
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Product.Any(_product => _product.ID == id);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditProductData(int id, Product productDataEdit)
        {
            if (id != productDataEdit.ID)
            {
                return BadRequest();
            }

            _dbContext.Entry(productDataEdit).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var _product = await _dbContext.Product.FindAsync(id);

            if (_product == null)
            {
                return NotFound();
            }

            _dbContext.Product.Remove(_product);

            await _dbContext.SaveChangesAsync();

            return Ok("Data Product berhasil dihapus");
        }

        [HttpGet("id")]
        public async Task<ActionResult<Product>> GetDetailProduct(int id)
        {
            var _product = await _dbContext.Product.FindAsync(id);

            if (_product == null)
            {

                return NotFound("Data Product tidak ditemukan");
            }

            return _product;
        }
    }
}
