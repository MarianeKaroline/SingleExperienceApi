using System;
using SingleExperience.Services.ProductServices;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.EmployeeServices;


namespace SingleExperience.Views
{
    class ClientHomeView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        EmployeeService employee = new EmployeeService(context);
        ClientService clientService = new ClientService(context);
        BoughtService boughtService = new BoughtService(context);
        ProductService productService = new ProductService(context);
        CartService cartService = new CartService(context);

        //Tela inicial
        public void Menu()
        {
            var selectedProduct = new ClientSelectedProductView();
            var signIn = new ClientSignInView();
            var signUp = new ClientSignUpView();
            var perfilClientView = new ClientPerfilView();
            var pefilEmployee = new EmployeePerfilView();
            var cart = new ClientCartView();

            var opc = 0;
            var invalid = true;
            var invalidCode = true;

            Console.WriteLine("\n0. Precisa de ajuda?");
            Console.WriteLine("1. Buscar por categoria");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {CountProduct})");
            if (Session.Length != 11)
            {
                Console.WriteLine("3. Fazer Login");
                Console.WriteLine("4. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("3. Ver perfil");
                Console.WriteLine("4. Desconectar-se");
            }
            Console.WriteLine("5. Selecionar um produto");
            Console.WriteLine("6. Área funcionário");
            Console.WriteLine("9. Sair do Sistema");
            while (invalid)
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (opc)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");
                    Console.WriteLine("Tecle enter para continuar");
                    Console.ReadLine();

                    ListProducts();
                    break;
                case 1:
                    Search();
                    break;
                case 2:
                    cart.ListCart();
                    break;
                case 3:                    
                    if (Session.Length == 11 && clientService.GetUser().Employee == false)
                    {
                        var listBought = boughtService.Show();
                        perfilClientView.Menu(listBought);
                    }
                    else if (Session.Length == 11 && clientService.GetUser().Employee == true)
                    {
                        pefilEmployee.Menu();
                    }
                    else
                    {
                        signIn.Login(true);
                    }
                    break;
                case 4:
                    if (Session.Length == 11)
                    {
                        Session = clientService.SignOut();
                        CountProduct = cartService.Total().TotalAmount;
                        ListProducts();
                    }
                    else
                    {
                        signUp.SignUp(true);
                    }
                    break;
                case 5:
                    Console.Write("\nDigite o código # do produto: "); 
                    while (invalidCode)
                    {
                        try
                        {
                            int code = int.Parse(Console.ReadLine());
                            if (productService.HasProduct(code))
                            {
                                selectedProduct.SelectedProduct(code);
                            }
                            else
                            {
                                Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                                Console.ReadKey();
                                Menu();
                            }
                            invalidCode = false;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Opção inválida, tente novamente.");
                            Console.WriteLine(e);
                        }
                    }
                    break;
                case 6:
                    if (Session.Length < 11)
                    {
                        signIn.Login(true);
                    }
                    else
                    {
                        pefilEmployee.Menu();
                    }
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu();
                    break;
            }
        }

        //Pesquisa
        public void Search()
        {
            var products = new ClientProductCategoryView();
            Console.Clear();

            Console.WriteLine("\nInício > Pesquisa\n");

            Console.WriteLine("0. Voltar Início");
            Console.WriteLine("1. Acessórios");
            Console.WriteLine("2. Celular");
            Console.WriteLine("3. Computador");
            Console.WriteLine("4. Notebook");
            Console.WriteLine("5. Tablets");
            Console.WriteLine("6. Precisa de ajuda?");
            Console.WriteLine("9. Sair do Sistema");

            //Colocar try catch
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 0:
                    ListProducts();
                    break;
                case 1:
                    products.Category(CategoryEnum.Acessorio);
                    break;
                case 2:
                    products.Category(CategoryEnum.Celular);
                    break;
                case 3:
                    products.Category(CategoryEnum.Computador);
                    break;
                case 4:
                    products.Category(CategoryEnum.Notebook);
                    break;
                case 5:
                    products.Category(CategoryEnum.Tablets);
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");

                    Menu();
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Search();
                    break;
            }
        }

        //Listar produtos na página inicial 
        public void ListProducts()
        {
            var products = productService.ListProducts();
            var j = 41;

            Console.Clear();

            Console.WriteLine("\nInício\n");

            if (Session.Length == 11)
            {
                Console.WriteLine($"Usuário: {clientService.ClientName(Session)}");
            }

            products.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"| #{p.ProductId}{new string(' ', j - 2 - p.ProductId.ToString().Length)}|");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"| R${p.Price.ToString("F2")}{new string(' ', j - 6 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
            Menu();
        }
    }
}
