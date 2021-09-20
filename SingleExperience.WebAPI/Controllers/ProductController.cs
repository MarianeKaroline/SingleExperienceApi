﻿using SingleExperience.Repository.Services.ProductServices.Models;
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

        [HttpGet("get/all")]
        public async Task<List<ListProductsModel>> GetAll()
        {
            return await product.GetAll();
        }

        [HttpGet]
        public async Task<List<BestSellingModel>> Get()
        {
            return await product.Get();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<List<ProductCategoryModel>> GetProductCategory(CategoryEnum categoryId)
        {
            return await product.GetProductCategory(categoryId);
        }

        [HttpGet("{productId}")]
        public async Task<ProductSelectedModel> GetSelected(int productId)
        {
            return await product.GetSelected(productId);
        }

        [HttpGet("category")]
        public async Task<List<CategoriesModel>> GetCategory()
        {
            return await product.GetCategories();
        }

        [HttpPost("newproduct")]
        public async Task<bool> Add([FromBody] AddNewProductModel addProduct)
        {
            return await product.Add(addProduct);
        }

        [HttpPut("{productId:int}/{available:bool}")]
        public async Task<bool> EditAvailable(int productId, bool available)
        {
            return await product.EditAvailable(productId, available);
        }

        [HttpPut("{productId:int}/{rating:decimal}")]
        public async Task<bool> Rating(int productId, decimal rating)
        {
            return await product.Rating(productId, rating);
        }
    }
}
