using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;
using System;

namespace SingleExperience.Views
{
    class EmployeePerfilView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private EmployeeService employeeService = new EmployeeService(context);

        public void Menu()
        {
            EmployeeInventoryView employeeInventory = new EmployeeInventoryView();
            EmployeeListAllBoughtView allBought = new EmployeeListAllBoughtView();
            EmployeeRegisterView signUp = new EmployeeRegisterView();
            ClientHomeView homeView = new ClientHomeView();

            bool validate = true;
            int opc = 0;

            var aux = employeeService.Access(Session);

            Console.Clear();

            Console.WriteLine("\nAdministrador\n");

            Console.WriteLine("0. Voltar para o início");
            Console.WriteLine("1. Ver lista de compras");
            if (aux.AccessRegister)
            {
                Console.WriteLine("2. Ver funcionários cadastrados");
            }
            if (aux.AccessInventory)
            {
                Console.WriteLine("3. Estoque");
            }
            Console.WriteLine("4. Desconectar-se");
            Console.WriteLine("9. Sair do Sistema");
            while (validate)
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (opc)
            {
                case 0:
                    homeView.ListProducts();
                    break;
                case 1:
                    allBought.Bought();
                    break;
                case 2:
                    if (aux.AccessRegister)
                    {
                        signUp.ListEmployee();
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu();
                    }
                    break;

                case 3:
                    if (aux.AccessInventory)
                    {
                        employeeInventory.Inventory();
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu();
                    }
                    break;

                case 4:
                    Session = employeeService.SignOut();
                    homeView.ListProducts();
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
    }
}
