
using SingleExperience.Entities;
using SingleExperience.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SingleExperience.Services.ProductServices;

namespace SingleExperience.Views
{
    class ClientCartView : SessionModel
    {
        static Context.SingleExperience context = new Context.SingleExperience();
        private CartService cartService = new CartService(context);
        private ProductService productService = new ProductService(context);


        public void ListCart()
        {
            Console.Clear();
            var total = cartService.Total();
            var j = 41;

            Console.WriteLine($"\nInício > Carrinho\n");

            var itens = cartService.ShowProducts(StatusProductEnum.Ativo);


            itens.ForEach(p =>
            {
                var product = productService.ListAllProducts().FirstOrDefault(i => i.ProductId == p.ProductId);

                if (p.StatusId == StatusProductEnum.Ativo)
                {
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                    Console.WriteLine($"|{product.Name}{new string(' ', j - product.Name.Length)}|");
                    Console.WriteLine($"|Qtd: {p.Amount}{new string(' ', j - 3 - p.Amount.ToString().Length)}|");
                    Console.WriteLine($"|R${product.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - product.Price.ToString().Length)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                }
            });

            Console.WriteLine($"\nSubtotal ({total.TotalAmount} itens): R${total.TotalPrice.ToString("F2")}");
            Menu(itens);
        }

        public void Menu(List<ProductCartModel> list)
        {
            ClientProductCategoryView productCategory = new ClientProductCategoryView();
            ClientSendingAddressView address = new ClientSendingAddressView();
            ClientHomeView inicio = new ClientHomeView();
            ClientService client = new ClientService(context);
            ClientSignUpView signUp = new ClientSignUpView();
            ClientSignInView signIn = new ClientSignInView();

            var op = 0;
            var code = 0;
            var invalid = true;
            var invalidCode = true;
            var invalidCodeRemove = true;
            var total = cartService.Total();
            var category = cartService.ShowProducts(StatusProductEnum.Ativo)
                .Select(p => p.CategoryId)
                .FirstOrDefault();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Buscar por categoria");

            if (CountProduct > 0)
            {
                Console.WriteLine($"2. Voltar para a categoria: {(CategoryEnum)category}");
                Console.WriteLine("3. Adicionar o produto mais uma vez ao carrinho");
                Console.WriteLine("4. Excluir um produto");
                Console.WriteLine("5. Finalizar compra");
            }

            if (Session.Length < 11)
            {
                Console.WriteLine("6. Fazer Login");
                Console.WriteLine("7. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("6. Desconectar-se");
            }
            Console.WriteLine("9. Sair do Sistema");
            while (invalid)
            {
                try
                {
                    op = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }

            }

            switch (op)
            {
                case 0:
                    inicio.ListProducts();
                    break;
                case 1:
                    inicio.Search();
                    break;
                case 2:
                    productCategory.Category(category);
                    break;
                case 3:
                    Console.Write("\nPor favor digite o código do produto #");
                    while (invalidCode)
                    {
                        try
                        {
                            code = int.Parse(Console.ReadLine());
                            invalidCode = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Opção inválida, tente novamente.");
                        }
                    }
                    list.ForEach(p =>
                    {
                        if (p.ProductId == code)
                        {
                            var cartModel = new CartModel();
                            cartModel.ProductId = p.ProductId;
                            cartModel.UserId = Session;
                            cartModel.Name = p.Name;
                            cartModel.CategoryId = p.CategoryId;
                            cartModel.StatusId = StatusProductEnum.Ativo;
                            cartModel.Price = p.Price;

                            if (Session.Length < 11)
                            {
                                cartService.AddItensMemory(cartModel);
                            }
                            else
                            {
                                cartService.AddProduct(cartModel);
                            }
                        }
                    });

                    CountProduct = cartService.Total().TotalAmount;

                    Console.WriteLine("\n\nProduto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();

                    ListCart();
                    break;
                case 4:
                    Console.Write("\nPor favor digite o codigo do produto #");
                    while (invalidCodeRemove)
                    {
                        try
                        {
                            if (CountProduct == 0)
                            {
                                Console.WriteLine("Carrinho vazio. Não é mais possível remover um item (Aperte enter para continuar)");
                                Console.ReadKey();
                                ListCart();
                            }
                            else
                            {
                                int codeRemove = int.Parse(Console.ReadLine());
                                cartService.RemoveItem(codeRemove);
                                CountProduct = cartService.Total().TotalAmount;

                                Console.WriteLine("\n\nProduto removido com sucesso (Aperte enter para continuar)");
                                Console.ReadKey();

                                invalidCodeRemove = false;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Opção inválida, tente novamente.");
                            Console.WriteLine(e);
                        }
                    }


                    ListCart();
                    break;
                case 5:
                    if (Session.Length < 11)
                    {
                        Console.WriteLine("\nVocê não está logado!!\n");
                        Console.WriteLine("1. Fazer login");
                        Console.WriteLine("2. Cadastrar-se");
                        Console.WriteLine("3. Cancelar");
                        Console.WriteLine("9. Sair do Sistema");
                        while (true)
                        {
                            try
                            {
                                int opc = int.Parse(Console.ReadLine());
                                switch (opc)
                                {
                                    case 1:
                                        signIn.Login(false);
                                        break;
                                    case 2:
                                        signUp.SignUp(false);
                                        break;
                                    case 3:
                                        ListCart();
                                        break;
                                    case 9:
                                        Environment.Exit(0);
                                        break;
                                    default:
                                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                                        Console.ReadKey();
                                        Menu(list);
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Opção inválida, tente novamente.");
                            }
                        }
                    }
                    else
                    {
                        address.Address();
                    }
                    break;
                case 6:
                    if (Session.Length == 11)
                    {
                        Session = client.SignOut();
                        CountProduct = cartService.Total().TotalAmount;
                        ListCart();
                    }
                    else
                    {
                        CountProduct = total.TotalAmount;
                        signIn.Login(true);
                    }
                    break;
                case 7:
                    CountProduct = total.TotalAmount;
                    signUp.SignUp(true);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(list);
                    break;
            }
        }
    }
}
