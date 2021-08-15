using SingleExperience.Services.ProductServices;
using SingleExperience.Enums;
using System;
using System.Globalization;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.CartServices;

namespace SingleExperience.Views
{
    class ClientProductCategoryView : SessionModel
    {
        private static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private CartService cartService = new CartService(context);
        private ClientService clientService = new ClientService(context);
        private ProductService productService = new ProductService(context);

        //Chama ListaProdutos pela Categoria
        public void Category(CategoryEnum id)
        {
            Console.Clear();
            Console.WriteLine($"\nInício > Pesquisa > {id}\n");
            var categoryId = Convert.ToInt32(id);

            ListProducts(id);
            Menu(id);
        }

        //Menu dos Produtos
        public void Menu(CategoryEnum id)
        {
            ClientSelectedProductView selectedProduct = new ClientSelectedProductView();
            ClientSignInView signIn = new ClientSignInView();
            ClientSignUpView signUp = new ClientSignUpView();
            ClientCartView cartView = new ClientCartView();
            ClientHomeView inicio = new ClientHomeView();

            var op = 0;
            var invalid = true;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {CountProduct})");
            if (Session.Length < 11)
            {
                Console.WriteLine("3. Fazer Login");
                Console.WriteLine("4. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("3. Desconectar-se");
            }
            Console.WriteLine("9. Sair do Sistema");
            Console.WriteLine("\n5. Selecionar um produto");
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
                    cartView.ListCart();
                    break;
                case 3:
                    if (Session.Length == 11)
                    {
                        Session = clientService.SignOut();
                        CountProduct = cartService.Total().TotalAmount;
                        Category(id);
                    }
                    else
                    {
                        signIn.Login(true);
                    }
                    break;
                case 4:
                    signUp.SignUp(true);
                    break;
                case 5:
                    Console.Write("\nDigite o código # do produto: ");
                    int code = int.Parse(Console.ReadLine());
                    if (productService.HasProduct(code))
                    {
                        selectedProduct.SelectedProduct(code);
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu(id);
                    }
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(id);
                    break;
            }


        }
        //Listar Produtos selecionado
        public void ListProducts(CategoryEnum categoryId)
        {
            var itemCategory = productService.ListProductCategory(categoryId);
            var j = 41;

            itemCategory.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"|R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
        }
    }
}
