using System;
using System.Collections.Generic;
using System.Text;
using SingleExperience.Entities.Enums;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;

namespace SingleExperience.Views
{
    class EmployeeListAllBoughtView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private EmployeeStatusBoughtView productStatus = new EmployeeStatusBoughtView();
        private EmployeeService employeeService = new EmployeeService(context);
        private BoughtService boughtService = new BoughtService(context);

        public void Bought()
        {
            int j = 51;

            Console.Clear();

            Console.WriteLine("\nAdministrador > Lista Compras\n");

            boughtService.ListAll().ForEach(i =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|Pedido em {i.DateBought}{new string(' ', j - $"Pedido em {i.DateBought}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|Status do pedido: {i.StatusId}{new string(' ', j - $"Status do pedido: {(StatusBoughtEnum)i.StatusId}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|Endereço de entrega{new string(' ', j - "Endereço de entrega".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|{i.ClientName}{new string(' ', j - i.ClientName.Length)}|");
                Console.WriteLine($"|{i.Street}, {i.Number}{new string(' ', j - i.Street.Length - 2 - i.Number.Length)}|");
                Console.WriteLine($"|{i.City} - {i.State}{new string(' ', j - i.City.Length - 3 - i.State.Length)}|");
                Console.WriteLine($"|{i.Cep}{new string(' ', j - i.Cep.Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|Forma de pagamento{new string(' ', j - $"Forma de pagamento".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");

                if (i.PaymentMethod == PaymentEnum.CreditCard)
                    Console.WriteLine($"|(Crédito) com final {i.NumberCard.Substring(12)}{new string(' ', j - $"(Crédito) com final {i.NumberCard.Substring(12)}".Length)}|");
                else if (i.PaymentMethod == PaymentEnum.BankSlip)
                    Console.WriteLine($"|(Boleto){new string(' ', j - $"(Boleto)".Length)}|");
                else
                    Console.WriteLine($"|(PIX){new string(' ', j - $"(PIX)".Length)}|");

                Console.WriteLine($"|{new string(' ', j)}|");

                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|Resumo do pedido{new string(' ', j - "Resumo do pedido".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|Subtotal do(s) item(ns): R$ {i.TotalPrice.ToString("F2")}{new string(' ', j - $"Subtotal do(s) item(ns): R$ {i.TotalPrice.ToString("F2")}".Length)}|");
                Console.WriteLine($"|Frete: R$ 0,00{new string(' ', j - "Frete: R$ 0,00".Length)}|");
                Console.WriteLine($"|Total do Pedido: R$ {i.TotalPrice.ToString("F2")}{new string(' ', j - $"Total do Pedido: R$ {i.TotalPrice.ToString("F2")}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");

                i.Itens.ForEach(k =>
                {
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"|{k.ProductName}{new string(' ', j - k.ProductName.Length)}|");
                    Console.WriteLine($"|Qtde: {k.Amount}{new string(' ', j - $"Qtde: {k.Amount}".Length)}|");
                    Console.WriteLine($"|R${k.Price}{new string(' ', j - $"R${k.Price}".Length)}|");
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                });
            });
            Menu();
        }

        public void Menu()
        {
            ClientHomeView homeView = new ClientHomeView();

            bool validate = true;
            int opc = 0;

            Console.WriteLine("\n\n0. Voltar para o início");
            Console.WriteLine("1. Ver produtos com confirmação pendente");
            Console.WriteLine("2. Ver produtos com pagamento pendente");
            Console.WriteLine("3. Ver produtos cancelados");
            Console.WriteLine("4. Desconectar-se");
            Console.WriteLine("9. Sair do programa");
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
                    productStatus.Bought(StatusBoughtEnum.ConfirmacaoPendente);
                    break;
                case 2:
                    productStatus.Bought(StatusBoughtEnum.PagamentoPendente);
                    break;
                case 3:
                    productStatus.Bought(StatusBoughtEnum.Cancelado);
                    break;
                case 4:
                    Session = employeeService.SignOut();
                    homeView.ListProducts();
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida, tente novamente.");
                    Console.WriteLine("\nTente novamente");
                    Menu();
                    break;
            }
        }
    }
}
