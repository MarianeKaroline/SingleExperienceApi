using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Services.ProductServices;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Domain.Entities;
using SingleExperience.Domain.Common;
using SingleExperience.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SingleExperience.Domain;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SingleExperience.Repository.Services.BoughtServices
{
    public class BoughtService : Session
    {
        protected readonly Context context;
        private ClientService clientService;
        private ProductService productService;
        private CartService cartService;

        public BoughtService(Context context)
        {
            this.context = context;
            productService = new ProductService(context);
            cartService = new CartService(context);
            clientService = new ClientService(context);
        }

        public async Task<List<BoughtModel>> GetAll()
        {
            var listProducts = new List<BoughtModel>();
            var listBought = await context.Bought.ToListAsync();

            //Lista todas as compras
            listBought.ForEach(i =>
            {
                var teste = context.Enjoyer.Where(j => j.Cpf == i.Cpf).FirstOrDefaultAsync();
                var client = context.Enjoyer.FirstOrDefault(j => j.Cpf == i.Cpf);
                var address = context.Address.Where(j => j.Cpf == i.Cpf);
                var card = context.CreditCard.Where(j => j.Cpf == i.Cpf);
                var boughtModel = new BoughtModel();
                boughtModel.Itens = new List<ProductBoughtModel>();

                boughtModel.ClientName = client.Name;
                var aux = address.FirstOrDefault(j => j.AddressId == i.AddressId);

                boughtModel.Cep = aux.PostCode;
                boughtModel.Street = aux.Street;
                boughtModel.Number = aux.Number;
                boughtModel.City = aux.City;
                boughtModel.State = aux.State;

                boughtModel.BoughtId = i.BoughtId;
                boughtModel.PaymentMethod = i.PaymentEnum;

                if (i.PaymentEnum == PaymentEnum.CreditCard)
                    boughtModel.NumberCard = card.FirstOrDefault(j => j.CreditCardId == i.CreditCardId).Number;

                boughtModel.TotalPrice = i.TotalPrice;
                boughtModel.DateBought = i.DateBought;
                boughtModel.StatusId = i.StatusBoughtEnum;

                boughtModel.Itens = GetProduct(i.BoughtId)
                    .Select(j => new ProductBoughtModel()
                    {
                        ProductId = j.ProductId,
                        ProductName = context.Product.ToList().FirstOrDefault(i => i.ProductId == j.ProductId).Name,
                        Amount = j.Amount,
                        Price = context.Product.ToList().FirstOrDefault(i => i.ProductId == j.ProductId).Price,
                        BoughtId = j.BoughtId
                    })
                    .ToList();

                listProducts.Add(boughtModel);
            });

            return listProducts;
        }

        public List<Bought> Get()
        {
            //Irá procurar a compra pelo cpf do cliente
            return context.Bought
                .Select(p => new Bought
                {
                    BoughtId = p.BoughtId,
                    TotalPrice = p.TotalPrice,
                    AddressId = p.AddressId,
                    PaymentEnum = p.PaymentEnum,
                    CreditCardId = p.CreditCardId,
                    Cpf = p.Cpf,
                    StatusBoughtEnum = p.StatusBoughtEnum,
                    DateBought = p.DateBought
                })
                .Where(p => p.Cpf == SessionId)
                .ToList();
        }

        public List<ProductBought> GetProduct(int boughtId)
        {
            //Irá procurar o os produtos da compra pela compra id
            return context.ProductBought
                .Select(p => new ProductBought
                {
                    ProductId = p.ProductId,
                    Amount = p.Amount,
                    BoughtId = p.BoughtId,
                })
                .Where(p => p.BoughtId == boughtId)
                .ToList();
        }

        public async Task Add(AddBoughtModel addBought)
        {
            StatusBoughtEnum statusBought = 0;
            Bought bought;

            if (addBought.PaymentId == PaymentEnum.BankSlip)
                statusBought = StatusBoughtEnum.PagamentoPendente;
            else
                statusBought = StatusBoughtEnum.ConfirmacaoPendente;

            using (var transaction = await context.BeginTransactionAsync())
            {
                try
                {
                    //Adiciona compra
                    if (addBought.PaymentId == PaymentEnum.CreditCard)
                    {
                        bought = new Bought()
                        {
                            TotalPrice = addBought.TotalPrice,
                            AddressId = addBought.AddressId,
                            PaymentEnum = addBought.PaymentId,
                            CreditCardId = clientService.GetCard().Where(p => p.CreditCardId == addBought.CreditCardId).FirstOrDefault().CreditCardId,
                            Cpf = SessionId,
                            StatusBoughtEnum = statusBought,
                            DateBought = DateTime.Now
                        };
                    }
                    else
                    {
                        bought = new Bought()
                        {
                            TotalPrice = addBought.TotalPrice,
                            AddressId = addBought.AddressId,
                            PaymentEnum = addBought.PaymentId,
                            Cpf = SessionId,
                            StatusBoughtEnum = statusBought,
                            DateBought = DateTime.Now
                        };
                    }

                    await context.Bought.AddAsync(bought);
                    await context.SaveChangesAsync();

                    AddProduct();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine(e);
                }
            }            
        }

        public void AddProduct()
        {
            var getCart = cartService.Get();
            var listItens = new List<ProductCart>();

            //Adiciona na lista os produtos que estão ativos no carrinho
            listItens.Add(cartService.GetProducts(getCart.CartId)
                .Where(i => i.StatusProductEnum == StatusProductEnum.Active)
                .FirstOrDefault());

            listItens.ForEach(i =>
            {
                //Adiciona itens do carrinho na tabela ProductBought
                var ProductBought = new ProductBought()
                {
                    ProductId = i.ProductId,
                    Amount = i.Amount,
                    BoughtId = context.Bought.OrderByDescending(j => j.BoughtId).FirstOrDefault().BoughtId
                };

                context.ProductBought.Add(ProductBought);
            });

            context.SaveChanges();
        }

        public async Task UpdateStatus(int boughtId, StatusBoughtEnum status)
        {
            var getBought = await context.Bought.FirstOrDefaultAsync(i => i.BoughtId == boughtId);

            getBought.StatusBoughtEnum = status;

            context.Bought.Update(getBought);
            await context.SaveChangesAsync();
        }

        public async Task<PreviewBoughtModel> Preview(BuyModel bought, int addressId)
        {
            var preview = new PreviewBoughtModel();
            var client = clientService.GetUser();
            var address = clientService.GetAddress();
            var card = clientService.GetCard();
            var cart = cartService.Get();
            var itens = cartService.GetProducts(cart.CartId);
            var listProducts = new List<ProductCartModel>();

            //Pega alguns atributos do cliente
            preview.FullName = client.Name;
            preview.Phone = client.Phone;

            //Pega alguns atributos do endereço
            var aux = address
                .FirstOrDefault(i => i.AddressId == addressId);

            preview.Cep = aux.PostCode;
            preview.Street = aux.Street;
            preview.Number = aux.Number;
            preview.City = aux.City;
            preview.State = aux.State;

            preview.Method = bought.Method;

            if (bought.Method == PaymentEnum.CreditCard)
            {
                card
                    .Where(i => i.Number.ToString().Contains(bought.Confirmation))
                    .ToList()
                    .ForEach(i =>
                    {
                        preview.NumberCard = i.Number.ToString();
                    });
            }
            else if (bought.Method == PaymentEnum.BankSlip)
            {
                var a = bought.Confirmation.Length;
                preview.Code = bought.Confirmation;
            }
            else
            {
                preview.Pix = bought.Confirmation;
            }

            if (bought.Ids.Count > 0)
            {
                bought.Ids.ForEach(i =>
                {
                    listProducts.Add(cartService.ShowProducts().Result
                                    .Where(j => j.ProductId == i)
                                    .FirstOrDefault());
                });
                preview.Itens = listProducts;
            }
            else
            {
                preview.Itens = await cartService.ShowProducts();
            }

            return preview;
        }

        public async Task<List<BoughtModel>> Show()
        {
            var client = clientService.GetUser();
            var address = clientService.GetAddress();
            var card = clientService.GetCard();
            var cart = cartService.Get();
            var itens = cartService.GetProducts(cart.CartId);
            var listProducts = new List<BoughtModel>();

            var listBought = Get();

            //Listar as compras do cliente
            if (listBought.Count > 0)
            {
                listBought.ForEach(i =>
                {
                    var boughtModel = new BoughtModel();
                    boughtModel.Itens = new List<ProductBoughtModel>();

                    boughtModel.ClientName = client.Name;
                    var aux = address.FirstOrDefault(j => j.AddressId == i.AddressId);

                    boughtModel.Cep = aux.PostCode;
                    boughtModel.Street = aux.Street;
                    boughtModel.Number = aux.Number;
                    boughtModel.City = aux.City;
                    boughtModel.State = aux.State;

                    boughtModel.BoughtId = i.BoughtId;
                    boughtModel.PaymentMethod = (PaymentEnum)i.PaymentEnum;

                    if (i.PaymentEnum == PaymentEnum.CreditCard)
                        boughtModel.NumberCard = card.FirstOrDefault(j => j.CreditCardId == i.CreditCardId).Number;

                    boughtModel.TotalPrice = i.TotalPrice;
                    boughtModel.DateBought = i.DateBought;
                    boughtModel.StatusId = i.StatusBoughtEnum;


                    boughtModel.Itens = GetProduct(i.BoughtId)
                        .Select(j => new ProductBoughtModel()
                        {
                            ProductId = j.ProductId,
                            ProductName = context.Product.ToList().FirstOrDefault(i => i.ProductId == j.ProductId).Name,
                            Amount = j.Amount,
                            Price = context.Product.ToList().FirstOrDefault(i => i.ProductId == j.ProductId).Price,
                            BoughtId = j.BoughtId
                        })
                        .ToList();

                    listProducts.Add(boughtModel);
                });
            }

            return listProducts;
        }

        public async Task<bool> Exist(int boughtId)
        {
            //Verifica se o número da compra que o cliente digitou está correto
            return await context.Bought.AnyAsync(i => i.BoughtId == boughtId);
        }        

        public async Task<List<BoughtModel>> Status(StatusBoughtEnum status)
        {
            var list = await GetAll();
            return list.Where(i => i.StatusId == status).ToList();
        }
    }
}
