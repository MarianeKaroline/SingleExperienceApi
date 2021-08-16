using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain;
using SingleExperience.Domain.Entities;
using SingleExperience.Domain.Enums;
using SingleExperience.Repository.Common.Domain;
using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Services.ProductServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            SessionId = "0";
            Itens = new List<ProductCart>();
        }

        public Cart Get()
        {
            //Irá procurar o carrinho pelo userId
            return context.Cart
                .Select(p => new Cart
                {
                    CartId = p.CartId,
                    Cpf = p.Cpf,
                    DateCreated = p.DateCreated
                })
                .FirstOrDefault(p => p.Cpf == SessionId);
        }
        

        public List<ProductCart> ListItens(int cartId)
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
        

        public async Task<TotalCartModel> Total()
        {
            var itens = await ShowProducts();
            var total = new TotalCartModel();

            if (SessionId.Length == 11)
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


        public int Add()
        {
            var currentCart = Get();
            var cartId = 0;

            //Criar Carrinho
            if (currentCart == null)
            {
                var cart = new Cart()
                {
                    Cpf = SessionId,
                    DateCreated = DateTime.Now
                };

                context.Cart.Add(cart);
                context.SaveChanges();

                cartId = context.Cart.FirstOrDefault(i => i.Cpf == SessionId).CartId;
            }
            else
            {
                cartId = currentCart.CartId;
            }

            return cartId;
        }
        

        public async Task AddProduct(CartModel cartModel)
        {
            if (SessionId.Length < 11)
            {
                AddItensMemory(cartModel);
            }
            else
            {
                var cartId = Add();
                var exist = ExistItem(cartModel);
                if (Itens.Count > 0)
                {
                    await PassProducts();
                    exist = true;
                }

                if (!exist)
                {
                    var item = new ProductCart()
                    {
                        ProductId = cartModel.ProductId,
                        CartId = cartId,
                        Amount = 1,
                        StatusProductEnum = cartModel.StatusId
                    };

                    await context.ProductCart.AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }
        }


        public async Task PassProducts()
        {
            var linesCart = new List<string>();
            var exist = false;

            //Verify if cliente already has a cart
            var cartId = Add();
            var listItensCart = ListItens(cartId).ToList(); 

            //Verify if product is already in the cart
            if (listItensCart.Count() > 0)
            {
                Itens.ForEach(i =>
                {
                    CartModel cartModel = new CartModel()
                    {
                        ProductId = i.ProductId,
                        Cpf = SessionId,
                    };

                    exist = ExistItem(cartModel);
                });
            }

            //Passa the product to cart
            if (!exist)
            {
                Itens.ForEach(async i =>
                {
                    var item = new ProductCart()
                    {
                        ProductId = i.ProductId,
                        CartId = cartId,
                        Amount = i.Amount,
                        StatusProductEnum = i.StatusProductEnum
                    };

                    await context.ProductCart.AddAsync(item);
                });
                await context.SaveChangesAsync();
            }
        }

        
        public async Task RemoveItem(int productId)
        {
            var getCart = Get();
            var getItem = ListItens(Get().CartId).FirstOrDefault(i => i.ProductId == productId);
            var sum = 0;
            var count = 0;

            if (SessionId.Length == 11)
            {
                if (getItem.Amount > 1 && count == 0)
                {
                    sum = getItem.Amount - 1;
                    await EditAmount(productId, sum);
                    count++;
                }
                else if (getItem.Amount == 1)
                {
                    await EditStatusProduct(productId, StatusProductEnum.Deleted);
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


        public async Task EditStatusProduct(int productId, StatusProductEnum status)
        {
            var getItem = ListItens(Get().CartId).FirstOrDefault(i => i.ProductId == productId);
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

        
        public async Task EditAmount(int productId, int sub)
        {
            var getItem = ListItens(Get().CartId).FirstOrDefault(i => i.ProductId == productId);
            var lines = new List<string>();

            getItem.Amount = sub;

            context.ProductCart.Update(getItem);
            await context.SaveChangesAsync();
        }        

        
        public async Task<List<ProductCartModel>> ShowProducts()
        {
            var prod = new List<ProductCartModel>();
            var product = context.Product.ToList();

            if (SessionId.Length == 11)
            {
                try
                {
                    prod = await context.ProductCart
                        .Where(i => i.StatusProductEnum == StatusProductEnum.Active && i.CartId == Get().CartId)
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = product.FirstOrDefault(i => i.ProductId == j.ProductId).Name,
                            CategoryId = product.FirstOrDefault(i => i.ProductId == j.ProductId).CategoryEnum,
                            StatusId = j.StatusProductEnum,
                            Amount = j.Amount,
                            Price = product.FirstOrDefault(i => i.ProductId == j.ProductId).Price
                        })
                        .ToListAsync();
                }
                catch (IOException e)
                {
                    Console.WriteLine("Ocorreu um erro");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                prod = Itens
                        .Where(i => i.StatusProductEnum == StatusProductEnum.Active)
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = product.FirstOrDefault(i => i.ProductId == j.ProductId).Name,
                            CategoryId = product.FirstOrDefault(i => i.ProductId == j.ProductId).CategoryEnum,
                            StatusId = j.StatusProductEnum,
                            Amount = j.Amount,
                            Price = product.FirstOrDefault(i => i.ProductId == j.ProductId).Price
                        })
                        .ToList();
            }

            return prod;
        }

        
        public async Task<bool> CallEditStatus(List<BuyProductModel> products)
        {
            var buy = false;

            products.ForEach(async i =>
            {
                await EditStatusProduct(i.ProductId, i.Status);
                buy = true;
            });

            return buy;
        }

        
        public bool ExistItem(CartModel cartModel)
        {
            var cartId = Add();
            var listItensCart = ListItens(cartId);
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
                            await EditStatusProduct(j.ProductId, StatusProductEnum.Active);
                            await EditAmount(j.ProductId, i.Amount);
                            exist = true;
                        }
                        else if (j.ProductId == i.ProductId)
                        {
                            await EditAmount(j.ProductId,i.Amount + 1);
                            exist = true;
                        }
                    });
                });
            }
            else
            {
                listItensCart.ForEach(async j =>
                {
                    if (j.ProductId == cartModel.ProductId && j.StatusProductEnum != StatusProductEnum.Active)
                    {
                        await EditStatusProduct(cartModel.ProductId, StatusProductEnum.Active);
                        exist = true;
                    }
                    else if (j.ProductId == cartModel.ProductId)
                    {
                        sum += j.Amount;
                        await EditAmount(cartModel.ProductId, sum);
                        exist = true;
                    }
                });
            }

            return exist;
        }

        
        public void AddItensMemory(CartModel cart)
        {
            var sum = 1;

            //Verify if cart productId is different of zero
            if (cart.ProductId != 0)
            {
                var aux = Itens
                        .Where(i => i.ProductId == cart.ProductId)
                        .FirstOrDefault();

                if (aux == null)
                {
                    var item = new ProductCart()
                    {
                        ProductId = cart.ProductId,
                        Amount = sum,
                        StatusProductEnum = cart.StatusId
                    };
                    Itens.Add(item);
                }
                else
                {
                    Itens.ForEach(i =>
                    {
                        i.ProductId = cart.ProductId;
                        i.Amount += sum;
                        i.StatusProductEnum = cart.StatusId;
                    });
                }
            }
        }
    }
}
