using SingleExperience.Views;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices;
using System.Collections.Generic;
using SingleExperience.Entities;

namespace SingleExperience
{
    public class Program : SessionModel
    {        
        static void Main(string[] args)
        {
            Context.SingleExperience context = new Context.SingleExperience();
            //Carrinho memória

            //Chama a função para pegar o IP do PC
            ClientService clientService = new ClientService(context);
            Session = clientService.GetIP();
            Itens = new List<ProductCart>();

            //Arrumar a herança
            //Chama a função para pegar a quantidade que está no carrinho
            CartService cartService = new CartService(context);
            CountProduct = cartService.Total().TotalAmount;

            //Chama a home para ser exibida inicialmente
            ClientHomeView inicio = new ClientHomeView();
            inicio.ListProducts();
        }

    }
}
