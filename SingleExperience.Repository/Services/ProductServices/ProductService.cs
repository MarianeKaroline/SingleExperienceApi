using SingleExperience.Domain;
using SingleExperience.Domain.Entities;
using SingleExperience.Domain.Enums;
using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Repository.Services.ProductServices.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleExperience.Services.ProductServices
{
    public class ProductService
    {
        protected readonly Context context;

        public ProductService(Context context)
        {
            this.context = context;
        }

        public List<ListProductsModel> ListAllProducts()
        {
            return context.Product
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
                .ToList();
        }

        public List<BestSellingModel> ListProducts()
        {
            return context.Product
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
                .ToList();
        }

        public List<CategoryModel> ListProductCategory(CategoryEnum categoryId)
        {
            return context.Product
                .Where(p => p.Available == true && p.CategoryEnum == categoryId)
                .Select(i => new CategoryModel()
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    CategoryId = i.CategoryEnum,
                    Available = i.Available
                })
                .ToList();
        }

        public ProductSelectedModel SelectedProduct(int productId)
        {
            return context.Product
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
             .FirstOrDefault();
        }

        public bool HasProduct(int code)
        {
            return context.Product.Any(i => i.ProductId == code);
        }

        public bool Confirm(List<ProductBoughtModel> products)
        {
            products.ForEach(j =>
            {
                var product = context.Product
                    .Where(i => i.ProductId == j.ProductId)
                    .FirstOrDefault();

                product.Amount -= j.Amount;
                product.Ranking += j.Amount;

                context.Product.Update(product);
                context.SaveChanges();
            });

            return true;
        }

        public bool EditAvailable(int productId, bool available)
        {
            var product = context.Product
                .FirstOrDefault(i => i.ProductId == productId && i.Available != available);

            product.Available = available;

            context.Product.Update(product);
            context.SaveChanges();

            return true;
        }

        public void Add(AddNewProductModel newProduct)
        {
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

            context.Product.Add(model);
            context.SaveChanges();
        }

        public void Rating(int productId, decimal rating)
        {
            var product = context.Product.FirstOrDefault(i => i.ProductId == productId);

            product.Rating += rating;

            context.Product.Update(product);
            context.SaveChanges();
        }
    }
}
