using SingleExperience.Entities.Enums;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class ClientProductsBoughtView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private ProductService productService = new ProductService(context);

        public void ProductsBought(List<BoughtModel> boughtModels, int boughtId)
        {
            int j = 51;

            Console.Clear();

            Console.WriteLine("\nSua conta > Seus pedidos > Dados do pedido\n");

            boughtModels
                .Where(k => k.BoughtId == boughtId)
                .ToList()
                .ForEach(i =>
                {
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|Pedido em {i.DateBought}{new string(' ', j - $"Pedido em {i.DateBought}".Length)}|");
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"|Status do pedido: {(StatusBoughtEnum)i.StatusId}{new string(' ', j - $"Status do pedido: {(StatusBoughtEnum)i.StatusId}".Length)}|");
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
                        Console.WriteLine($"|#{k.ProductId}{new string(' ', j - $"#{k.ProductId}".Length)}|"); 
                        Console.WriteLine($"|{k.ProductName}{new string(' ', j - k.ProductName.Length)}|"); 
                        Console.WriteLine($"|Qtde: {k.Amount}{new string(' ', j - $"Qtde: {k.Amount}".Length)}|");
                        Console.WriteLine($"|R${k.Price}{new string(' ', j - $"R${k.Price}".Length)}|");
                        Console.WriteLine($"|{new string(' ', j)}|");
                        Console.WriteLine($"+{new string('-', j)}+");
                    });
                });
            Menu(boughtModels, boughtId);
        }
        public void Menu(List<BoughtModel> boughtModels, int boughtId)
        {
            var homeView = new ClientHomeView();
            var perfilView = new ClientPerfilView();
            bool validate = true;
            int opc = 0;

            Console.WriteLine("\n\n0. Voltar para o início");
            Console.WriteLine("1. Voltar para sua conta");
            Console.WriteLine("2. Avaliar pedido");
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
                    perfilView.Menu(boughtModels);
                    break;
                case 2:
                    Console.Write("\nDigite o código do produto: ");
                    var productId = int.Parse(Console.ReadLine());

                    Console.Write("Digite sua avaliação: (1 - 5)");
                    var rating = decimal.Parse(Console.ReadLine());

                    productService.Rating(productId, rating);

                    Console.WriteLine("Produto avaliado com sucesso. (Tecle enter para continuar)");
                    Console.ReadKey();
                    ProductsBought(boughtModels, boughtId);
                    break;
                default:
                    break;
            }

            if (opc == 0)
            {
                homeView.ListProducts();
            }
            else
            {
                perfilView.Menu(boughtModels);
            }
        }
    }
}
