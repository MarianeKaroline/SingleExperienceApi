using SingleExperience.Entities;
using SingleExperience.Services.ClientServices.Models;
using SingleExperience.Services.UserServices;
using System.Collections.Generic;
using System.Linq;

namespace SingleExperience.Services.ClientServices
{
    public class ClientService : UserService
    {
        protected readonly Context.SingleExperience context;

        public ClientService(Context.SingleExperience context) : base(context)
        {
            this.context = context;
        }

        //Address
        public List<Address> ListAddress(string cpf)
        {
            return context.Address
                .Select(i => new Address
                {
                    AddressId = i.AddressId,
                    PostCode = i.PostCode,
                    Street = i.Street,
                    Number = i.Number,
                    City = i.City,
                    State = i.State,
                    Cpf = i.Cpf
                })
                .Where(i => i.Cpf == cpf)
                .ToList();
        }

        //Card
        public List<CreditCard> ListCard(string cpf)
        {
            return context.CreditCard
                    .Where(i => i.Cpf == cpf)
                    .Select(i => new CreditCard
                    {
                        Number = i.Number,
                        Name = i.Name,
                        ShelfLife = i.ShelfLife,
                        Cvv = i.Cvv,
                        Cpf = i.Cpf
                    })
                    .ToList();
        }


        /* Cadastro */
        //Client
        public bool SignUpClient(SignUpModel client)
        {
            var existClient = GetUser();

            if (existClient == null)
            {
                SignUp(client);
            }

            return existClient == null;
        }

        //Address
        public int AddAddress(AddressModel addressModel)
        {
            var address = new Entities.Address()
            {
                PostCode = addressModel.Cep,
                Street = addressModel.Street,
                Number = addressModel.Number,
                City = addressModel.City,
                State = addressModel.State,
                Cpf = addressModel.Cpf
            };

            context.Address.Add(address);
            context.SaveChanges();

            return context.Address.FirstOrDefault().AddressId;
        }

        public int IdInserted(string cpf)
        {
            return ListCard(cpf).OrderByDescending(j => j.CreditCardId).FirstOrDefault().CreditCardId;
        }

        //Card
        public void AddCard(string cpf, CardModel card)
        {
            var existCard = ListCard(cpf).FirstOrDefault(i => i.Number == card.CardNumber);
            var lines = new List<string>();

            if (existCard == null)
            {
                var creditCard = new CreditCard()
                {
                    Number = card.CardNumber,
                    Name = card.Name,
                    ShelfLife = card.ShelfLife,
                    Cvv = card.CVV,
                    Cpf = cpf
                };

                context.CreditCard.Add(creditCard);
                context.SaveChanges();
            }
        }


        //Puxa o nome do cliente
        public string ClientName(string cpf)
        {
            return GetUser().Name;
        }

        //Verifica se o cliente possui cartão de crédito
        public bool HasCard(string cpf)
        {
            return ListCard(cpf).Any();
        }


        //Verifica se cliente já cadastrou algum endereço
        public bool HasAddress(string cpf)
        {
            return context.Address.Any(i => i.Cpf == cpf);
        }

        //Traz todos os cartões cadastrados do usuário
        public List<ShowCardModel> ShowCards(string cpf)
        {
            return ListCard(cpf)
                .Select(i => new ShowCardModel
                {
                    CardNumber = i.Number.ToString(),
                    Name = i.Name,
                    ShelfLife = i.ShelfLife
                })
                .ToList();
        }


        //Traz todos os endereços do usuário
        public List<ShowAddressModel> ShowAddress(string cpf)
        {
            var client = GetUser();
            var listAddress = ListAddress(cpf);

            return listAddress
                .Select(i => new ShowAddressModel
                {
                    ClientName = client.Name,
                    ClientPhone = client.Phone,
                    AddressId = i.AddressId,
                    Cep = i.PostCode,
                    Street = i.Street,
                    Number = i.Number,
                    City = i.City,
                    State = i.State
                })
                .ToList();
        }
    }
}
