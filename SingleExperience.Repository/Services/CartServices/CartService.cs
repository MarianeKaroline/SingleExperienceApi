using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Services.ProductServices;
using SingleExperience.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain.Enums;
using System.Collections.Generic;
using SingleExperience.Domain;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;

namespace SingleExperience.Services.CartServices
{
    public class CartService
    {
        protected readonly Context context;
        private ProductService productService;

        public CartService(Context context)
        {
            this.context = context;
            productService = new ProductService(context);
        }

        public Cart Get(string sessionId)
        {
            //Irá procurar o carrinho pelo userId
            return context.Cart
                .Select(p => new Cart
                {
                    CartId = p.CartId,
                    Cpf = p.Cpf,
                    DateCreated = p.DateCreated
                })
                .FirstOrDefault(p => p.Cpf == sessionId);
        }

        public List<ProductCart> GetProducts(int cartId)
        {
            //Retorna a lista de produtos do carrinho
            return context.ProductCart
                .Select(i => new ProductCart
                {
                    ProductCartId = i.ProductCartId,
                    ProductId = i.ProductId,
                    CartId = i.CartId,
                    Amount = i.Amount,
                    StatusProductEnum = i.StatusProductEnum
                })
                .Where(i => i.CartId == cartId)
                .ToList();
        }

        public async Task<List<PaymentModel>> Payment()
        {
            return await context.Payment
                .Select(i => new PaymentModel()
                {
                    PaymentEnum = i.PaymentEnum,
                    Description = i.Description
                })
                .ToListAsync();
        }

        public async Task<List<ProductCartModel>> ShowProducts(string sessionId)
        {
            var prod = new List<ProductCartModel>();
            if (Get(sessionId) != null)
                prod = await context.ProductCart
                    .Where(p => p.StatusProductEnum == StatusProductEnum.Active && p.CartId == Get(sessionId).CartId)
                    .Select(j => new ProductCartModel()
                    {
                        ProductId = j.ProductId,
                        Name = j.Product.Name,
                        CategoryId = j.Product.CategoryEnum,
                        Category = j.Product.Category,
                        StatusId = j.StatusProductEnum,
                        Amount = j.Amount,
                        Price = j.Product.Price,
                        Image = j.Product.Image
                    })
                    .ToListAsync();

            if (prod == null)
                prod = new List<ProductCartModel>();

            return prod;
        }

        public async Task<TotalCartModel> Total(string sessionId)
        {
            var itens = await ShowProducts(sessionId);
            var total = new TotalCartModel();

            total.TotalAmount = itens
                .Where(item => item.StatusId == StatusProductEnum.Active)
                .Sum(item => item.Amount);
            total.TotalPrice = itens
                .Where(item => item.StatusId == StatusProductEnum.Active)
                .Sum(item => item.Price * item.Amount);

            return total;
        }

        public bool ExistProduct(int productId, string sessionId)
        {
            var cartId = Add(sessionId);
            var listItensCart = GetProducts(cartId);
            var exist = false;
            var sum = 1;

            listItensCart.ForEach(j =>
            {
                if (j.ProductId == productId && j.StatusProductEnum != StatusProductEnum.Active)
                {
                    EditStatus(productId, StatusProductEnum.Active, sessionId);
                    exist = true;
                }
                else if (j.ProductId == productId)
                {
                    sum += j.Amount;
                    EditAmount(productId, sum, sessionId);
                    exist = true;
                }
            });


            return exist;
        }

        public int Add(string sessionId)
        {
            var currentCart = Get(sessionId);
            var cartId = 0;

            //Criar Carrinho
            if (currentCart == null)
            {
                var cart = new Cart()
                {
                    Cpf = sessionId,
                    DateCreated = DateTime.Now
                };

                context.Cart.Add(cart);
                context.SaveChanges();

                cartId = context.Cart.FirstOrDefault(i => i.Cpf == sessionId).CartId;
            }
            else
            {
                cartId = currentCart.CartId;
            }

            return cartId;
        }

        public async Task<bool> AddProduct(int productId, string sessionId)
        {
            var cartId = Add(sessionId);
            var exist = ExistProduct(productId, sessionId);

            if (!exist)
            {
                var item = new ProductCart()
                {
                    ProductId = productId,
                    CartId = cartId,
                    Amount = 1,
                    StatusProductEnum = StatusProductEnum.Active
                };

                await context.ProductCart.AddAsync(item);
                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task RemoveProduct(int productId, string sessionId)
        {
            var getItem = new ProductCart();
            var getCart = Get(sessionId);
            if (getCart != null)
            {
                getItem = GetProducts(Get(sessionId).CartId).FirstOrDefault(i => i.ProductId == productId);
            }
            var sum = 0;
            var count = 0;

            if (getItem.Amount > 1 && count == 0)
            {
                sum = getItem.Amount - 1;
                EditAmount(productId, sum, sessionId);
                count++;
            }
            else if (getItem.Amount == 1)
            {
                EditStatus(productId, StatusProductEnum.Deleted, sessionId);
            }
        }

        public async Task RemoveProducts(int productId, string sessionId)
        {
            var cart = await context.Cart.FirstOrDefaultAsync(i => i.Cpf == sessionId);
            var getItem = await context.ProductCart.FirstOrDefaultAsync(i => i.ProductId == productId && i.CartId == cart.CartId);

            getItem.Amount = 1;
            getItem.StatusProductEnum = StatusProductEnum.Deleted;

            context.ProductCart.Update(getItem);
            await context.SaveChangesAsync();
            
        }

        public void EditStatus(int productId, StatusProductEnum status, string sessionId)
        {
            var cartId = context.Cart.FirstOrDefault(i => i.Cpf == sessionId).CartId;
            var getItem = context.ProductCart.FirstOrDefault(i => i.ProductId == productId && i.CartId == cartId);
            var auxAmount = 0;

            if (status == StatusProductEnum.Active)
                auxAmount = 1;
            else
                auxAmount = getItem.Amount;

            getItem.Amount = auxAmount;
            getItem.StatusProductEnum = status;

            context.ProductCart.Update(getItem);
            context.SaveChanges();
        }

        public void EditAmount(int productId, int sub, string sessionId)
        {
            var cartId = context.Cart.FirstOrDefault(i => i.Cpf == sessionId).CartId;
            var getItem = context.ProductCart.FirstOrDefault(i => i.ProductId == productId && i.CartId == cartId);


            var lines = new List<string>();

            getItem.Amount = sub;

            context.ProductCart.Update(getItem);
            context.SaveChanges();
        }

        public async Task<List<ProductCartModel>> PassItems(int productId, int amount, string sessionId)
        {
            var item = await context.ProductCart.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (item == null)
            {
                var cartId = Add(sessionId);
                var exist = ExistProduct(productId, sessionId);

                if (!exist)
                {
                    var product = new ProductCart()
                    {
                        ProductId = productId,
                        CartId = cartId,
                        Amount = amount,
                        StatusProductEnum = StatusProductEnum.Active
                    };

                    await context.ProductCart.AddAsync(product);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                if (item.ProductId == productId && item.StatusProductEnum != StatusProductEnum.Active)
                {
                    EditStatus(productId, StatusProductEnum.Active, sessionId);
                    EditAmount(productId, amount, sessionId);
                }
                else if (item.ProductId == productId)
                {
                    amount += item.Amount;
                    EditAmount(productId, amount, sessionId);
                }
            }

            return await ShowProducts(sessionId);
        }

        public bool CallEditStatus(List<BuyProductModel> products, string sessionId)
        {
            var buy = false;

            products.ForEach(i =>
            {
                EditStatus(i.ProductId, i.Status, sessionId);
                buy = true;
            });

            return buy;
        }
        
    }
}
