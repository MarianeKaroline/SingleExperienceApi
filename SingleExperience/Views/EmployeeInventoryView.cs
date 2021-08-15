using SingleExperience.Enums;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;
using SingleExperience.Services.ProductServices;
using SingleExperience.Services.ProductServices.Models;
using System;

namespace SingleExperience.Views
{
    class EmployeeInventoryView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private ProductService productService = new ProductService(context);
        private EmployeeService employeeService = new EmployeeService(context);
        private AddNewProductModel newProduct = new AddNewProductModel();

        public void Inventory()
        {
            var j = 51;
            var invalid = true;
            char opc = '\0';

            Console.Clear();

            Console.WriteLine("\nAdministrador > Estoque\n");

            productService.ListAllProducts().ForEach(i =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|Produto n° {i.ProductId}{new string(' ', j - $"Produto n° {i.ProductId}".Length)}|");
                Console.WriteLine($"|Nome Produto: {i.Name}{new string(' ', j - $"Nome Produto: {i.Name}".Length)}|");
                Console.WriteLine($"|Preço: R${i.Price.ToString("F2")}{new string(' ', j - $"Preço: R${i.Price}".Length)}|");
                Console.WriteLine($"|Qtde: {i.Amount}{new string(' ', j - $"Qtde: {i.Amount}".Length)}|");
                Console.WriteLine($"|Categoria: {i.CategoryId}{new string(' ', j - $"Categoria: {i.CategoryId}".Length)}|");
                Console.WriteLine($"|Ranking: {i.Ranking}{new string(' ', j - $"Ranking: {i.Ranking}".Length)}|");
                Console.WriteLine($"|Disponivel: {i.Available}{new string(' ', j - $"Disponivel: {i.Available}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });

            Console.Write("\nAdicionar um novo produto? (s/n) \n");
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
                    Add();
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
            EmployeeRegisterView signUp = new EmployeeRegisterView();
            ClientHomeView homeView = new ClientHomeView();
            EmployeeListAllBoughtView allBought = new EmployeeListAllBoughtView();

            bool validate = true;
            int opc = 0;

            var aux = employeeService.Access(Session);

            Console.WriteLine("0. Voltar para o início");
            Console.WriteLine("1. Ver lista de compras");
            if (aux.AccessRegister)
            {
                Console.WriteLine("2. Ver funcionários cadastrados");
            }
            Console.WriteLine("3. Editar disponibilidade do produto");
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
                    signUp.ListEmployee();
                    break;
                case 3:
                    Console.Write("\nDigite o código do produto: ");
                    var opt = int.Parse(Console.ReadLine());

                    Console.WriteLine("\n1. Ativar produto");
                    Console.WriteLine("\n2. Desativar produto");
                    var op = int.Parse(Console.ReadLine());

                    switch (op)
                    {
                        case 1:
                            productService.EditAvailable(opt, true);
                            break;
                        case 2:
                            productService.EditAvailable(opt, false);
                            break;
                        default:
                            break;
                    }


                    Inventory();
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

        public void Add()
        {
            var validate = true;

            Console.Clear();

            Console.WriteLine("\nAdministrador > Estoque > Cadastrar novo produto\n");

            Console.Write("Nome do produto: ");
            newProduct.Name = Console.ReadLine();

            while (validate)
            {
                try
                {
                    Console.Write("Preço: ");
                    newProduct.Price = decimal.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            Console.Write("Detalhe do produto: ");
            newProduct.Detail = Console.ReadLine();

            validate = true;
            while (validate)
            {
                try
                {
                    Console.Write("Quantidade: ");
                    newProduct.Amount = int.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            validate = true;
            while (validate)
            {
                try
                {
                    Console.Write("Categoria Id: ");
                    newProduct.CategoryId = (CategoryEnum)int.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            validate = true;
            while (validate)
            {
                try
                {
                    Console.Write("Ranking: ");
                    newProduct.Ranking = int.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            validate = true;
            while (validate)
            {
                try
                {
                    Console.Write("Disponibilidade: ");
                    newProduct.Available = bool.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            validate = true;
            while (validate)
            {
                try
                {
                    Console.Write("Avaliação: ");
                    newProduct.Rating = decimal.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            productService.Add(newProduct);
            Inventory();
        }
    }
}
