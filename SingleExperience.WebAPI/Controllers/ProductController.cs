using Microsoft.AspNetCore.Mvc;
using SingleExperience.Domain.Enums;
using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Repository.Services.ProductServices.Models;
using SingleExperience.Services.ProductServices;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleExperience.WebAPI.Controllers
{
    [Route("singleexperience")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        protected readonly ProductService product;

        public ProductController(ProductService product) => this.product = product;

        // GET: singleexperience/manager/products
        [HttpGet("manager/products")]
        public async Task<List<ListProductsModel>> ListAllProducts()
        {
            return await product.ListAllProducts();
        }

        // GET: singleexperience/home
        [HttpGet("home")]
        public async Task<List<BestSellingModel>> ListProduct()
        {
            return await product.ListProducts();
        }

        // GET singleexperience/category/computer
        [HttpGet("category/{categoryId}")]
        public async Task<List<CategoryModel>> ListProductCategory(CategoryEnum categoryId)
        {
            return await product.ListProductCategory(categoryId);
        }

        // GET singleexperience/product-5
        [HttpGet("product-{id}")]
        public async Task<ProductSelectedModel> SelectedProduct(int id)
        {
            return await product.SelectedProduct(id);
        }

        // GET api/singleexperience/5
        [HttpGet("{productId}")]
        public async Task<bool> HasProduct(int productId)
        {
            return await product.HasProduct(productId);
        }

        // POST singleexperience/newproduct
        [HttpPost("newproduct")]
        public async Task<ActionResult> Add([FromBody] AddNewProductModel addProduct)
        {
            await product.Add(addProduct);

            return Ok("WORK!!!");
        }

        // PUT singleexperience
        [HttpPut]
        public async Task<bool> Confirm([FromBody] List<ProductBoughtModel> products)
        {
            return await product.Confirm(products);
        }

        // PUT singleexperience/5/false
        [HttpPut("{productId:int}/{available:bool}")]
        public async Task<bool> EditAvailable(int productId, bool available)
        {
            return await product.EditAvailable(productId, available);
        }

        // PUT singleexperience/5/4.5
        [HttpPut("{productId:int}/{rating:decimal}")]
        public async Task Rating(int productId, decimal rating)
        {
            await product.Rating(productId, rating);
        }
    }
}
