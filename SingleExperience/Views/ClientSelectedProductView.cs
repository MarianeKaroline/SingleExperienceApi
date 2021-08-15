using SingleExperience.Services.ProductServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Enums;
using System;
using System.Globalization;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices.Model;

using System.Collections.Generic;
using SingleExperience.Entities;

namespace SingleExperience.Views
{
    class ClientSelectedProductView : SessionModel
    {
        private static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private ProductService productService = new ProductService(context);
        private CartService cartService = new CartService(context);
        private ClientService clientService = new ClientService(context);

        //Listar Produtos
        public void SelectedProduct(int productId)
        {
            Console.Clear();
            var product = productService.SelectedProduct(productId);
            var j = 61;
            var category = product.CategoryId;

            Console.WriteLine($"\nInício > Pesquisa > {category} > {product.Name}\n");

            var aux = product.Detail.Split(';');

            Console.WriteLine($"+{new string('-', j)}+");
            Console.WriteLine($"|#{product.ProductId}{new string(' ', j - 1 - product.ProductId.ToString().Length)}|");
            Console.WriteLine($"|*{product.Rating.ToString("F1", CultureInfo.InvariantCulture)}{new string(' ', j - 3 - product.Rating.ToString().Length)}|");
            Console.WriteLine($"|{product.Name}{new string(' ', j - product.Name.ToString().Length)}|");
            Console.WriteLine($"|R${product.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - product.Price.ToString().Length)}|");
            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"|Detalhes{new string(' ', j - "Detalhes".Length)}|");

            for (int i = 0; i < aux.Length; i++)
            {
                Console.WriteLine($"|{aux[i]}{new string(' ', j - aux[i].Length)}|");
            }

            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"|Quantidade em estoque: {product.Amount}{new string(' ', j - "Quantidade em estoque".Length - 2 - product.Amount.ToString().Length)}|");
            Console.WriteLine($"+{new string('-', j)}+");


            Menu(product, productId);
        }

        //Mostra Menu
        public void Menu(ProductSelectedModel list, int productId)
        {
            ClientSignInView signIn = new ClientSignInView();
            ClientSignUpView signUp = new ClientSignUpView();
            ClientCartView cartView = new ClientCartView();
            ClientProductCategoryView categoryProduct = new ClientProductCategoryView();
            ClientHomeView inicio = new ClientHomeView();
            var category = (CategoryEnum)list.CategoryId;
            var op = 0;
            var invalid = true;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Voltar para a categoria: {category}");
            Console.WriteLine("3. Adicionar produto ao carrinho");
            Console.WriteLine($"4. Ver Carrinho (Quantidade: {CountProduct})");
            if (Session.Length < 11)
            {
                Console.WriteLine("5. Fazer Login");
                Console.WriteLine("6. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("5. Desconectar-se");
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
                    categoryProduct.Category(list.CategoryId);
                    break;
                case 3:
                    CartModel cartModel = new CartModel();

                    cartModel.ProductId = list.ProductId;
                    cartModel.UserId = Session;
                    cartModel.Name = list.Name;
                    cartModel.CategoryId = list.CategoryId;
                    cartModel.StatusId = StatusProductEnum.Ativo;
                    cartModel.Price = list.Price;

                    if (Session.Length < 11)
                    {

                        cartService.AddItensMemory(cartModel);
                    }
                    else
                    {
                        cartService.AddProduct(cartModel);
                    }

                    CountProduct = cartService.Total().TotalAmount;

                    Console.WriteLine("\nProduto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();
                    SelectedProduct(list.ProductId);
                    break;
                case 4:
                    cartView.ListCart();
                    break;
                case 5:
                    if (Session.Length == 11)
                    {
                        Session = clientService.SignOut();
                        CountProduct = cartService.Total().TotalAmount;
                        SelectedProduct(productId);
                    }
                    else
                    {
                        signIn.Login(true);
                    }
                    break;
                case 6:
                    signUp.SignUp(true);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
