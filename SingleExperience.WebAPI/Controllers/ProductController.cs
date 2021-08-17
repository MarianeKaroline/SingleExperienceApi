using SingleExperience.Repository.Services.ProductServices.Models;
using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Services.ProductServices;
using SingleExperience.Domain.Enums;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        protected readonly ProductService product;

        public ProductController(ProductService product) => this.product = product;

        [HttpGet("manager/products")]
        public async Task<List<ListProductsModel>> GetAll()
        {
            return await product.GetAll();
        }

        [HttpGet("home")]
        public async Task<List<BestSellingModel>> Get()
        {
            return await product.Get();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<List<CategoryModel>> GetCategory(CategoryEnum categoryId)
        {
            return await product.GetCategory(categoryId);
        }

        [HttpGet("product/{productId}")]
        public async Task<ProductSelectedModel> GetSelected(int productId)
        {
            return await product.GetSelected(productId);
        }

        [HttpGet("{productId}")]
        public async Task<bool> Exist(int productId)
        {
            return await product.Exist(productId);
        }

        [HttpPost("newproduct")]
        public async Task<bool> Add([FromBody] AddNewProductModel addProduct)
        {
            return await product.Add(addProduct);
        }

        [HttpPut]
        public async Task<bool> Confirm([FromBody] List<ProductBoughtModel> products)
        {
            return await product.Confirm(products);
        }

        [HttpPut("{productId:int}/{available:bool}")]
        public async Task<bool> EditAvailable(int productId, bool available)
        {
            return await product.EditAvailable(productId, available);
        }

        [HttpPut("{productId:int}/{rating:decimal}")]
        public async Task Rating(int productId, decimal rating)
        {
            await product.Rating(productId, rating);
        }
    }
}
