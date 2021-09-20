using SingleExperience.Repository.Services.ProductServices.Models;
using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain.Enums;
using System.Collections.Generic;
using SingleExperience.Domain;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SingleExperience.Services.ProductServices
{
    public class ProductService
    {
        protected readonly Context context;

        public ProductService(Context context)
        {
            this.context = context;
        }

        public async Task<List<ListProductsModel>> GetAll()
        {
            return await context.Product
                .Select(i => new ListProductsModel()
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    Amount = i.Amount,
                    Category = i.Category,
                    Ranking = i.Ranking,
                    Available = i.Available,
                    Image = i.Image
                })
                .ToListAsync();
        }

        public async Task<List<BestSellingModel>> Get()
        {
            return await context.Product
                .Where(p => p.Available == true)
                .OrderByDescending(p => p.Ranking)
                .Take(5)
                .Select(i => new BestSellingModel()
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    Available = i.Available,
                    Category = i.Category,
                    Ranking = i.Ranking,
                    Image = i.Image
                })
                .ToListAsync();
        }

        public async Task<List<ProductCategoryModel>> GetProductCategory(CategoryEnum categoryId)
        {
            return await context.Product
                .Where(p => p.Available == true && p.CategoryEnum == categoryId)
                .Select(i => new ProductCategoryModel()
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    Category = i.Category,
                    Available = i.Available,
                    Image = i.Image
                })
                .ToListAsync();
        }

        public async Task<List<CategoriesModel>> GetCategories()
        {
            return await context.Category
                .Select(i => new CategoriesModel()
                {
                    CategoryEnum = i.CategoryEnum,
                    Description = i.Description
                })
                .ToListAsync();
        }
        
        public async Task<ProductSelectedModel> GetSelected(int productId)
        {
            return await context.Product
             .Where(p => p.Available == true && p.ProductId == productId)
             .Select(i => new ProductSelectedModel()
             {
                 Category = i.Category,
                 ProductId = i.ProductId,
                 Name = i.Name,
                 Amount = i.Amount,
                 Detail = i.Detail,
                 Price = i.Price,
                 Rating = i.Rating,
                 Image = i.Image
             })
             .FirstOrDefaultAsync();
        }

        public async Task<bool> Add(AddNewProductModel newProduct)
        {
            newProduct.Validator();

            var model = new Product()
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                Detail = newProduct.Detail,
                Amount = newProduct.Amount,
                CategoryEnum = newProduct.CategoryId,
                Ranking = newProduct.Ranking,
                Available = newProduct.Available,
                Rating = newProduct.Rating,
                Image = newProduct.Image
            };

            await context.Product.AddAsync(model);
            await context.SaveChangesAsync();

            return true;
        }

        public void Confirm(int boughtId)
        {
            var products = context.ProductBought.Where(i => i.BoughtId == boughtId).ToList();

            products.ForEach(j =>
            {
                var product = context.Product
                    .Where(i => i.ProductId == j.ProductId)
                    .FirstOrDefault();

                product.Amount -= j.Amount;
                product.Ranking += 1;

                context.Product.Update(product);
            });

            context.SaveChanges();
        }

        public async Task<bool> EditAvailable(int productId, bool available)
        {
            var product = await context.Product
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.Available != available);

            if (product == null)
                throw new Exception($"O produto já está como {available}");

            product.Available = available;

            context.Product.Update(product);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Rating(int productId, decimal rating)
        {
            var product = context.Product.FirstOrDefault(i => i.ProductId == productId);
            var rate = product.Rating;

            product.Rating = (rating + product.Rating) / product.Ranking;

            context.Product.Update(product);
            await context.SaveChangesAsync();

            if (product.Rating == rate)
                return false;

            return true;
        }
    }
}
