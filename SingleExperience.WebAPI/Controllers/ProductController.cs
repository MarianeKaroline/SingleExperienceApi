using Microsoft.AspNetCore.Mvc;
using SingleExperience.Repository.Services.ProductServices.Models;
using SingleExperience.Services.ProductServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleExperience.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        protected readonly ProductService repository;

        public ProductController(ProductService repository)
        {
            this.repository = repository;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var bestSelling = repository.ListProducts();

                return Ok(bestSelling);
            }
            catch (System.Exception e)
            {
                return BadRequest($"Erro: {e}");
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var selectedProduct = repository.SelectedProduct(id);

                return Ok(selectedProduct);
            }
            catch (System.Exception e)
            {
                return BadRequest($"Erro: {e}");
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public ActionResult Post(AddNewProductModel product)
        {
            try
            {
                repository.Add(product);

                return Ok("WORK!!!");
            }
            catch (System.Exception e)
            {
                return BadRequest($"Erro: {e}");
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}/{available}")]
        public ActionResult Put(int productId, bool available)
        {
            try
            {
                var confirm = repository.EditAvailable(productId, available);

                return Ok(confirm);
            }
            catch (System.Exception e)
            {
                return BadRequest($"Erro: {e}");
            }

        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
