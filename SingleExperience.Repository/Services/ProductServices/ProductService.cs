using SingleExperience.Repository.Services.ProductServices.Models;
using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain.Enums;
using System.Collections.Generic;
using SingleExperience.Domain;
using System.Threading.Tasks;
using System.Linq;

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
                    CategoryId = i.CategoryEnum,
                    Ranking = i.Ranking,
                    Available = i.Available
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
                    Ranking = i.Ranking
                })
                .ToListAsync();
        }

        public async Task<List<CategoryModel>> GetCategory(CategoryEnum categoryId)
        {
            return await context.Product
                .Where(p => p.Available == true && p.CategoryEnum == categoryId)
                .Select(i => new CategoryModel()
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    CategoryId = i.CategoryEnum,
                    Available = i.Available
                })
                .ToListAsync();
        }
        
        public async Task<ProductSelectedModel> GetSelected(int productId)
        {
            return await context.Product
             .Where(p => p.Available == true && p.ProductId == productId)
             .Select(i => new ProductSelectedModel()
             {
                 CategoryId = i.CategoryEnum,
                 ProductId = i.ProductId,
                 Name = i.Name,
                 Amount = i.Amount,
                 Detail = i.Detail,
                 Price = i.Price,
                 Rating = i.Rating
             })
             .FirstOrDefaultAsync();
        }

        public async Task<bool> Exist(int productId)
        {
            return await context.Product.AnyAsync(i => i.ProductId == productId);
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
                Rating = newProduct.Rating
            };

            await context.Product.AddAsync(model);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Confirm(List<ProductBoughtModel> products)
        {
            products.ForEach(j =>
            {
                var product = context.Product
                    .Where(i => i.ProductId == j.ProductId)
                    .FirstOrDefault();

                product.Amount -= j.Amount;
                product.Ranking += j.Amount;

                context.Product.Update(product);
            });

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAvailable(int productId, bool available)
        {
            var product = await context.Product
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.Available != available);

            product.Available = available;

            context.Product.Update(product);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task Rating(int productId, decimal rating)
        {
            var product = context.Product.FirstOrDefault(i => i.ProductId == productId);

            product.Rating = (rating + product.Rating) / product.Ranking;

            context.Product.Update(product);
            await context.SaveChangesAsync();
        }
    }
}
