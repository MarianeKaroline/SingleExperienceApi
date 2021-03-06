using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;
using System;

namespace SingleExperience.Views
{
    class EmployeeRegisterView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private EmployeeListAllBoughtView allBought = new EmployeeListAllBoughtView();
        private EmployeeService employeeService = new EmployeeService(context);
        private EmployeeInventoryView employeeInventory = new EmployeeInventoryView();

        public void ListEmployee()
        {
            var signUp = new ClientSignUpView();
            var j = 51;
            char opc = '\0';
            var invalid = true;

            Console.Clear();

            Console.WriteLine("\nAdministrador > Lista funcionários\n");

            employeeService.List().ForEach(i =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|CPF: {i.Cpf}{new string(' ', j - $"CPF: {i.Cpf}".Length)}|");
                Console.WriteLine($"|Nome Completo: {i.FullName}{new string(' ', j - $"Nome Completo: {i.FullName}".Length)}|");
                Console.WriteLine($"|Email: {i.Email}{new string(' ', j - $"Email: {i.Email}".Length)}|");
                Console.WriteLine($"|Acesso ao inventário: {i.AccessInventory}{new string(' ', j - $"Acesso ao inventário: {i.AccessInventory}".Length)}|");
                Console.WriteLine($"|Acesso ao Regitro de funcionário: {i.RegisterEmployee}{new string(' ', j - $"Acesso ao Regitro de funcionário: {i.RegisterEmployee}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });

            Console.Write("Adicionar um novo funcionário? (s/n) \n");
            while (invalid)
            {
                try
                {
                    opc = char.Parse(Console.ReadLine().ToLower());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("\nOpção inválida, tente novamente.\n");
                }
            }

            switch (opc)
            {
                case 's':
                    signUp.SignUpEmployee();
                    break;
                case 'n':
                    Menu();
                    break;
                default:
                    break;
            }
        }
        public void Menu()
        {
            ClientHomeView homeView = new ClientHomeView();

            bool validate = true;
            int opc = 0;

            var aux = employeeService.Access(Session);

            Console.WriteLine("\n\n0. Voltar para o início");
            Console.WriteLine("1. Ver lista de compras");

            if (aux.AccessInventory)
            {
                Console.WriteLine("2. Estoque");
            }

            Console.WriteLine("3. Desconectar-se");
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
                case 3:
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
