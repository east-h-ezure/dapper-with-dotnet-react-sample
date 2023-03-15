using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace TestCrud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Product";
                var products = await connection.QueryAsync<Product>(query);

                return Ok(products);
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProduct(int id)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Product WHERE Id = @Id";
                var parameters = new { Id = id };
                var product = await connection.QueryFirstOrDefaultAsync<Product>(query, parameters);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO Product (Name, Price) VALUES (@Name, @Price)";
                await connection.ExecuteAsync(query, product);

                return Ok();
            }
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
