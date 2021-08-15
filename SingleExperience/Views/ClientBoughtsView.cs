using SingleExperience.Entities.Enums;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class ClientBoughtsView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private BoughtService boughtService = new BoughtService(context);

        public void Boughts(List<BoughtModel> boughtModels)
        {
            int j = 71;

            Console.Clear();

            Console.WriteLine("\nSua conta > Seus pedidos\n");

            boughtModels.ForEach(i =>
            {
                if (i.BoughtId > 0)
                {
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|Pedido n° : {i.BoughtId}{new string(' ', j - $"Pedido n° : {i.BoughtId}".Length)}|");
                    Console.WriteLine($"|Status do pedido: {(StatusBoughtEnum)i.StatusId}{new string(' ', j - $"Status do pedido: {(StatusBoughtEnum)i.StatusId}".Length)}|");
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"|Pedido Realizado       Total       Enviar Para{new string(' ', j - "Pedido Realizado       Total       Enviar Para".Length)}|");
                    Console.WriteLine($"|{i.DateBought}  R${i.TotalPrice.ToString("F2")}   {i.ClientName}{new string(' ', j - $"{i.DateBought}  R${i.TotalPrice.ToString("F2")}   {i.ClientName}".Length)}|");
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
                }
                else
                {
                    Console.WriteLine("Você não comprou nenhum produto ainda.");
                }
            });

            Menu(boughtModels);
        }

        public void Menu(List<BoughtModel> boughtModels)
        {
            ClientHomeView homeView = new ClientHomeView();
            ClientPerfilView perfilView = new ClientPerfilView();
            ClientProductsBoughtView productsBoughtView = new ClientProductsBoughtView();

            bool validate = true;
            int aux = 0;
            int opc = 0;

            boughtModels.ForEach(i =>
            {
                if (i.BoughtId == 0)
                {
                    aux++;
                }
            });

            Console.WriteLine("\n\n0. Voltar para o início");
            Console.WriteLine("1. Voltar para sua conta");
            Console.WriteLine("2. Ver detalhes da compra");

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
                    Console.WriteLine("\nDigite o número do perido\n");
                    var op = int.Parse(Console.ReadLine());

                    if (boughtService.HasBought(op))
                    {
                        productsBoughtView.ProductsBought(boughtModels, op);
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida, tente novamente.");
                        Console.WriteLine("\nTente novamente");
                        Menu(boughtModels);
                    }
                    break;
                default:
                    Console.WriteLine("Opção inválida, tente novamente.");
                    Console.WriteLine("\nTente novamente");
                    Menu(boughtModels);
                    break;
            }
        }
    }
}
