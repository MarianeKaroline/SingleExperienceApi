using SingleExperience.Enums;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Globalization;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.BoughtServices.Models;

namespace SingleExperience.Views
{
    class ClientPaymentMethodView : SessionModel
    {
        static SingleExperience.Context.SingleExperience context = new SingleExperience.Context.SingleExperience();
        private CartService cartService = new CartService(context);
        public int j = 41;
        private ClientService clientService = new ClientService(context);
        private CardModel cardModel = new CardModel();

        public void Methods(AddBoughtModel addBought)
        {
            var op = 0;
            var invalid = true;
            Console.Clear();

            Console.WriteLine("\nCarrinho > Informações pessoais > Endereço > Método de pagamento\n");

            Console.WriteLine("1. Cartão de Crédito");
            Console.WriteLine("2. Boleto");
            Console.WriteLine("3. Pix");
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
                case 1:
                    CreditCard(addBought, false);
                    break;
                case 2:
                    BankSlip(addBought);
                    break;
                case 3:
                    Pix(addBought);
                    break;
                default:
                    break;
            }
        }

        public void CreditCard(AddBoughtModel addBought, bool home)
        {
            ClientPreviewBoughtView preview = new ClientPreviewBoughtView();
            var op = 0;
            char opc = '\0';
            var invalid = true;

            if (home)
            {
                Console.Clear();

                Console.WriteLine("\nSua conta > Seus cartões cadastrados\n");
            }

            //Se o cliente tiver cartões cadastrado, irá mostrar para ele
            if (clientService.HasCard(Session))
            {
                clientService.ShowCards(Session)
                    .ForEach(p =>
                    {
                        Console.WriteLine($"\n(Crédito) com final {p.CardNumber.Substring(12)}        {p.Name}        {p.ShelfLife.ToString("MM/yyyy")}\n");
                    });

                if (home)
                {
                    Console.Write("Adicionar um novo cartão? (s/n) \n");
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
                            AddNewCreditCard(addBought, home);
                            break;
                        case 'n':
                            Menu();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.Write("Escolher um desses cartões: (s/n) ");
                    while (invalid)
                    {
                        try
                        {
                            opc = char.Parse(Console.ReadLine().ToLower());
                            invalid = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Opção inválida, tente novamente.");
                        }
                    }

                    switch (opc)
                    {
                        case 's':
                            invalid = true;
                            Console.Write("\nDigite o código do cartão #: ");
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
                            addBought.Payment = PaymentEnum.CreditCard;
                            addBought.CreditCardId = op;
                            preview.Bought(addBought);
                            break;
                        case 'n':
                            AddNewCreditCard(addBought, false);
                            break;
                        default:
                            Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                            Console.ReadKey();
                            CreditCard(addBought, home);
                            break;
                    }
                }
            }
            else
            {
                AddNewCreditCard(addBought, false);
            }
        }

        public void BankSlip(AddBoughtModel addBought)
        {
            ClientPreviewBoughtView preview = new ClientPreviewBoughtView();
            var numberCode = Guid.NewGuid();
            addBought.ReferenceCode = numberCode.ToString();

            addBought.Payment = PaymentEnum.BankSlip;
            preview.Bought(addBought);
        }

        public void Pix(AddBoughtModel addBought)
        {
            ClientPreviewBoughtView preview = new ClientPreviewBoughtView();
            var numberPix = Guid.NewGuid();
            addBought.ReferenceCode = numberPix.ToString();

            addBought.Payment = PaymentEnum.Pix;

            preview.Bought(addBought);
        }

        //Caso a edição do cartão seja do perfil, o home tem que ser true
        public void AddNewCreditCard(AddBoughtModel addBought, bool home)
        {
            ClientPreviewBoughtView preview = new ClientPreviewBoughtView();

            Console.WriteLine($"\n+{new string('-', j)}+\n");
            Console.Write("Novo Cartão\n");
            Console.Write("Número do cartão: ");
            cardModel.CardNumber = Console.ReadLine();
            Console.Write("Nome no cartão: ");
            cardModel.Name = Console.ReadLine();
            Console.Write("Data de expiração(01/2021): ");
            cardModel.ShelfLife = DateTime.ParseExact(Console.ReadLine(), "MM/yyyy", CultureInfo.InvariantCulture);
            Console.Write("Código de segurança(CVV): ");
            cardModel.CVV = Console.ReadLine();

            clientService.AddCard(Session, cardModel);

            if (home)
            {
                CreditCard(addBought, home);
            }
            else
            {
                addBought.CreditCardId = clientService.IdInserted(Session);

                addBought.Payment = PaymentEnum.CreditCard;
                preview.Bought(addBought);
            }
        }

        public void Menu()
        {
            ClientCartView cartView = new ClientCartView();
            var home = new ClientHomeView();
            var opc = 0;
            var invalid = true;

            Console.WriteLine("\n0. Precisa de ajuda?");
            Console.WriteLine("1. Voltar para o início");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {CountProduct})");
            Console.WriteLine("3. Desconectar-se");
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

                    home.ListProducts();
                    break;
                case 1:
                    home.ListProducts();
                    break;
                case 2:
                    cartView.ListCart();
                    break;
                case 3:
                    Session = clientService.SignOut();
                    CountProduct = cartService.Total().TotalAmount;
                    home.ListProducts();
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
