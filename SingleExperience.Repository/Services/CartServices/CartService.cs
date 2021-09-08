using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Services.ProductServices;
using SingleExperience.Domain.Entities;
using SingleExperience.Domain.Common;
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
    public class CartService : Session
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

        public async Task<List<ProductCartModel>> ShowProducts(string sessionId)
        {
            var prod = new List<ProductCartModel>();

            if (sessionId.Length == 11)
            {
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
            }
            else
            {
                prod = Itens
                        .Where(i => i.StatusProductEnum == StatusProductEnum.Active)
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = j.Product.Name,
                            CategoryId = j.Product.CategoryEnum,
                            StatusId = j.StatusProductEnum,
                            Amount = j.Amount,
                            Price = j.Product.Price,
                            Image = j.Product.Image
                        })
                        .ToList();
            }

            return prod;
        }

        public async Task<TotalCartModel> Total(string sessionId)
        {
            if (Itens == null)
                Itens = new List<ProductCart>();

            var itens = await ShowProducts(sessionId);
            var total = new TotalCartModel();

            if (sessionId.Length == 11)
            {
                total.TotalAmount = itens
                    .Where(item => item.StatusId == StatusProductEnum.Active)
                    .Sum(item => item.Amount);
                total.TotalPrice = itens
                    .Where(item => item.StatusId == StatusProductEnum.Active)
                    .Sum(item => item.Price * item.Amount);
            }
            else
            {
                if (itens.Count == 0)
                {
                    total.TotalAmount = 0;
                    total.TotalPrice = 0;
                }
                else
                {
                    total.TotalAmount = itens.Sum(item => item.Amount);
                    total.TotalPrice = itens.Sum(item => context.Product.ToList().FirstOrDefault(i => i.ProductId == item.ProductId).Price * item.Amount);
                }
            }

            return total;
        }

        public bool ExistProduct(int productId, string sessionId)
        {
            var cartId = Add(sessionId);
            var listItensCart = GetProducts(cartId);
            var exist = false;
            var sum = 1;

            if (Itens.Count() > 0)
            {
                listItensCart.ForEach(j =>
                {
                    Itens.ForEach(async i =>
                    {
                        if (j.ProductId == i.ProductId && j.StatusProductEnum != StatusProductEnum.Active)
                        {
                            await EditStatus(j.ProductId, StatusProductEnum.Active, sessionId);
                            await EditAmount(j.ProductId, i.Amount, sessionId);
                            exist = true;
                        }
                        else if (j.ProductId == i.ProductId)
                        {
                            await EditAmount(j.ProductId, i.Amount + 1, sessionId);
                            exist = true;
                        }
                    });
                });
            }
            else
            {
                listItensCart.ForEach(async j =>
                {
                    if (j.ProductId == productId && j.StatusProductEnum != StatusProductEnum.Active)
                    {
                        await EditStatus(productId, StatusProductEnum.Active, sessionId);
                        exist = true;
                    }
                    else if (j.ProductId == productId)
                    {
                        sum += j.Amount;
                        await EditAmount(productId, sum, sessionId);
                        exist = true;
                    }
                });
            }

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
            if (sessionId.Length != 11)
            {
                return AddToMemory(productId);
            }
            else
            {
                var cartId = Add(sessionId);
                var exist = ExistProduct(productId, sessionId);
                if (Itens.Count > 0)
                {
                    PassProducts(sessionId);
                    exist = true;
                    return true;
                }

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
            }
            return true;
        }

        public void PassProducts(string sessionId)
        {
            var linesCart = new List<string>();
            var exist = false;

            //Verify if cliente already has a cart
            var cartId = Add(sessionId);
            var listItensCart = GetProducts(cartId).ToList();

            //Verify if product is already in the cart
            if (listItensCart.Count() > 0)
            {
                Itens.ForEach(i =>
                {
                    exist = ExistProduct(i.ProductId, sessionId);
                });
            }

            //Passa the product to cart
            if (!exist)
            {
                Itens.ForEach(i =>
                {
                    var item = new ProductCart()
                    {
                        ProductId = i.ProductId,
                        CartId = cartId,
                        Amount = i.Amount,
                        StatusProductEnum = i.StatusProductEnum
                    };

                    context.ProductCart.Add(item);
                });
                context.SaveChangesAsync();
            }
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

            if (sessionId.Length == 11)
            {
                if (getItem.Amount > 1 && count == 0)
                {
                    sum = getItem.Amount - 1;
                    await EditAmount(productId, sum, sessionId);
                    count++;
                }
                else if (getItem.Amount == 1)
                {
                    await EditStatus(productId, StatusProductEnum.Deleted, sessionId);
                }
            }
            else
            {
                var aux = false;
                Itens.ForEach(i =>
                {
                    if (i.ProductId == productId && i.Amount > 1)
                    {
                        i.Amount -= 1;
                    }
                    else if (i.ProductId == productId && i.Amount == 1)
                    {
                        aux = true;
                    }
                });

                if (aux)
                {
                    Itens.RemoveAll(x => x.ProductId == productId);
                }
            }
        }

        public async Task EditStatus(int productId, StatusProductEnum status, string sessionId)
        {
            var getItem = GetProducts(Get(sessionId).CartId).FirstOrDefault(i => i.ProductId == productId);
            var auxAmount = 0;

            if (status == StatusProductEnum.Active)
                auxAmount = 1;
            else
                auxAmount = getItem.Amount;

            getItem.Amount = auxAmount;
            getItem.StatusProductEnum = status;

            context.ProductCart.Update(getItem);
            await context.SaveChangesAsync();
        }

        public async Task EditAmount(int productId, int sub, string sessionId)
        {
            var getItem = GetProducts(Get(sessionId).CartId).FirstOrDefault(i => i.ProductId == productId);
            var lines = new List<string>();

            getItem.Amount = sub;

            context.ProductCart.Update(getItem);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CallEditStatus(List<BuyProductModel> products, string sessionId)
        {
            var buy = false;

            products.ForEach(async i =>
            {
                await EditStatus(i.ProductId, i.Status, sessionId);
                buy = true;
            });

            return buy;
        }

        public bool AddToMemory(int productId)
        {
            var sum = 1;

            //Verify if cart productId is different of zero
            if (productId != 0)
            {
                var aux = Itens
                        .Where(i => i.ProductId == productId)
                        .FirstOrDefault();

                if (aux == null)
                {
                    var item = new ProductCart()
                    {
                        Product = context.Product.Where(i => i.ProductId == productId).FirstOrDefault(),
                        ProductId = productId,
                        Amount = sum,
                        StatusProductEnum = StatusProductEnum.Active
                    };
                    Itens.Add(item);
                }
                else
                {
                    Itens.ForEach(i =>
                    {
                        if (i.ProductId == context.Product.Where(j => j.ProductId == productId).FirstOrDefault().ProductId)
                        {
                            i.Amount += sum;
                        }
                    });
                }
            }

            if (Itens.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
