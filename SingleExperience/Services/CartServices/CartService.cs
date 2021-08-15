using SingleExperience.Entities;
using SingleExperience.Enums;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SingleExperience.Services.CartServices
{
    public class CartService : SessionModel
    {
        protected readonly Context.SingleExperience context;
        private ProductService productService;

        public CartService(Context.SingleExperience context)
        {
            this.context = context;
            productService = new ProductService(context);            
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
                .FirstOrDefault(p => p.Cpf == Session);
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
        

        public TotalCartModel Total()
        {
            var itens = ShowProducts(StatusProductEnum.Ativo);
            var total = new TotalCartModel();

            if (Session.Length == 11)
            {
                total.TotalAmount = itens
                    .Where(item => item.StatusId == StatusProductEnum.Ativo)
                    .Sum(item => item.Amount);
                total.TotalPrice = itens
                    .Where(item => item.StatusId == StatusProductEnum.Ativo)
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
                    total.TotalPrice = itens.Sum(item => productService.ListAllProducts().FirstOrDefault(i => i.ProductId == item.ProductId).Price * item.Amount);
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
                var cart = new Entities.Cart()
                {
                    Cpf = Session,
                    DateCreated = DateTime.Now
                };

                context.Cart.Add(cart);
                context.SaveChanges();

                cartId = context.Cart.FirstOrDefault(i => i.Cpf == Session).CartId;
            }
            else
            {
                cartId = currentCart.CartId;
            }

            return cartId;
        }
        

        public void AddProduct(CartModel cartModel)
        {
            var cartId = Add();
            var exist = ExistItem(cartModel);

            if (Itens.Count > 0)
            {
                PassItens();
                exist = true;
            }

            if (!exist)
            {
                var item = new Entities.ProductCart()
                {
                    ProductId = cartModel.ProductId,
                    CartId = cartId,
                    Amount = 1,
                    StatusProductEnum = cartModel.StatusId
                };

                context.ProductCart.Add(item);
                context.SaveChanges();
            }
        }


        public void PassItens()
        {
            var linesCart = new List<string>();
            var exist = false;

            //Verify if cliente already has a cart
            var cartId = Add();
            var listItensCart = ListItens(cartId);

            //Verify if product is already in the cart
            if (listItensCart.Count() > 0)
            {
                Itens.ForEach(i =>
                {
                    CartModel cartModel = new CartModel()
                    {
                        ProductId = i.ProductId,
                        UserId = Session,
                    };

                    exist = ExistItem(cartModel);
                });
            }

            //Passa the product to cart
            if (!exist)
            {
                Itens.ForEach(i =>
                {
                    var item = new Entities.ProductCart()
                    {
                        ProductId = i.ProductId,
                        CartId = cartId,
                        Amount = i.Amount,
                        StatusProductEnum = i.StatusProductEnum
                    };

                    context.ProductCart.Add(item);
                    context.SaveChanges();
                });
            }
        }

        
        public void RemoveItem(int productId)
        {
            var getCart = Get();
            var getItem = ListItens(getCart.CartId).FirstOrDefault(i => i.ProductId == productId);
            var sum = 0;
            var count = 0;

            if (Session.Length == 11)
            {
                if (getItem.Amount > 1 && count == 0)
                {
                    sum = getItem.Amount - 1;
                    EditAmount(productId, sum);
                    count++;
                }
                else if (getItem.Amount == 1)
                {
                    EditStatusProduct(productId, StatusProductEnum.Inativo);
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

        public void EditStatusProduct(int productId, StatusProductEnum status)
        {
            var getItem = context.ProductCart.FirstOrDefault(i => i.ProductId == productId && Get().CartId == i.CartId);
            var auxAmount = 0;

            if (status == StatusProductEnum.Ativo)
                auxAmount = 1;
            else
                auxAmount = getItem.Amount;

            getItem.Amount = auxAmount;
            getItem.StatusProductEnum = status;

            context.ProductCart.Update(getItem);
            context.SaveChanges();
        }

        
        public void EditAmount(int productId, int sub)
        {
            var getItem = context.ProductCart.FirstOrDefault(i => i.ProductId == productId && Get().CartId == i.CartId);
            var lines = new List<string>();

            getItem.Amount = sub;

            context.ProductCart.Update(getItem);
            context.SaveChanges();
        }        

        
        public List<ProductCartModel> ShowProducts(StatusProductEnum status)
        {
            var productService = new ProductService(context);
            var prod = new List<ProductCartModel>();
            var product = productService.ListAllProducts();

            if (Session.Length == 11)
            {
                var itensCart = ListItens(Get().CartId);

                try
                {
                    prod = itensCart
                        .Where(i => i.StatusProductEnum == status)
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = product.FirstOrDefault(i => i.ProductId == j.ProductId).Name,
                            CategoryId = product.FirstOrDefault(i => i.ProductId == j.ProductId).CategoryId,
                            StatusId = j.StatusProductEnum,
                            Amount = j.Amount,
                            Price = product.FirstOrDefault(i => i.ProductId == j.ProductId).Price
                        })
                        .ToList();
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
                        .Where(i => i.StatusProductEnum == status)
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = product.FirstOrDefault(i => i.ProductId == j.ProductId).Name,
                            CategoryId = product.FirstOrDefault(i => i.ProductId == j.ProductId).CategoryId,
                            StatusId = j.StatusProductEnum,
                            Amount = j.Amount,
                            Price = product.FirstOrDefault(i => i.ProductId == j.ProductId).Price
                        })
                        .ToList();
            }

            return prod;
        }

        
        public bool CallEditStatus(List<BuyProductModel> products)
        {
            var buy = false;

            products.ForEach(i =>
            {
                EditStatusProduct(i.ProductId, i.Status);
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
                    Itens.ForEach(i =>
                    {
                        if (j.ProductId == i.ProductId && j.StatusProductEnum != StatusProductEnum.Ativo)
                        {
                            EditStatusProduct(j.ProductId, StatusProductEnum.Ativo);
                            EditAmount(j.ProductId, i.Amount);
                            exist = true;
                        }
                        else if (j.ProductId == i.ProductId)
                        {
                            EditAmount(j.ProductId,i.Amount + 1);
                            exist = true;
                        }
                    });
                });
            }
            else
            {
                listItensCart.ForEach(j =>
                {
                    if (j.ProductId == cartModel.ProductId && j.StatusProductEnum != StatusProductEnum.Ativo)
                    {
                        EditStatusProduct(cartModel.ProductId, StatusProductEnum.Ativo);
                        exist = true;
                    }
                    else if (j.ProductId == cartModel.ProductId)
                    {
                        sum += j.Amount;
                        EditAmount(cartModel.ProductId, sum);
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
