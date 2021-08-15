using SingleExperience.Entities;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices.Models;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;

namespace SingleExperience.Views
{
    class ClientSignInView : SessionModel
    {
        private static Context.SingleExperience context = new Context.SingleExperience();
        private CartService cartService = new CartService(context);
        private SignInModel signIn = new SignInModel();
        private ClientService ClientService = new ClientService(context);


        public void Login(bool home)
        {
            ClientSendingAddressView addressView = new ClientSendingAddressView();
            EmployeePerfilView perfilEmployee = new EmployeePerfilView();
            ClientHomeView inicio = new ClientHomeView();

            Console.Clear();

            Console.WriteLine("Início > Login\n");
            Console.Write("Email: ");
            signIn.Email = Console.ReadLine();

            Console.Write("Senha: ");
            signIn.Password = ReadPassword();

            var enjoyer = ClientService.SignIn(signIn);

            //Verifica se usuário não existe
            if (enjoyer == null)
            {
                Console.WriteLine("\nUsuário não existe");
                Console.WriteLine("Tecle enter para continuar");
                Console.ReadKey();
                inicio.ListProducts();
            }

            Session = enjoyer.Cpf;

            //Verifica se o usuário é funcionário
            if (enjoyer.Employee)
            {
                perfilEmployee.Menu();
            }

            cartService.PassItens();
            Itens = new List<ProductCart>();
            CountProduct = cartService.Total().TotalAmount;

            //Verifica se página veio da home
            if (home)
            {
                Menu(home);
            }
            else
            {
                addressView.Address();
            }
        }

        public void Menu(bool home)
        {
            ClientHomeView inicio = new ClientHomeView();
            ClientCartView cart = new ClientCartView();
            var invalid = true;
            var op = 0;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {CountProduct})");
            if (Session.Length == 11)
            {
                Console.WriteLine("3. Desconectar-se");
            }
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
                    cart.ListCart();
                    break;
                case 3:
                    Session = ClientService.SignOut();
                    CountProduct = cartService.Total().TotalAmount;
                    inicio.ListProducts();
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(home);
                    break;
            }
        }

        public string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password;
        }

    }
}
