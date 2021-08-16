using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain;
using SingleExperience.Domain.Entities;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Services.UserServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleExperience.Services.ClientServices
{
    public class ClientService : UserService
    {
        protected readonly Context contexts;

        public ClientService(Context context) : base(context)
        {
            contexts = context;
        }

       
        public List<Address> ListAddress()
        {
            return contexts.Address
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
                .Where(i => i.Cpf == SessionId)
                .ToList();
        }

        
        public List<CreditCard> ListCard()
        {
            return contexts.CreditCard
                    .Where(i => i.Cpf == SessionId)
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

        
        public async Task<bool> SignUpClient(SignUpModel client)
        {
            var existClient = GetUser();

            if (existClient == null)
            {
                await SignUp(client);
            }

            return existClient == null;
        }

        
        public async Task<int> AddAddress(AddressModel addressModel)
        {
            var address = new Address()
            {
                PostCode = addressModel.Cep,
                Street = addressModel.Street,
                Number = addressModel.Number,
                City = addressModel.City,
                State = addressModel.State,
                Cpf = addressModel.Cpf
            };

            await contexts.Address.AddAsync(address);
            await contexts.SaveChangesAsync();

            return contexts.Address.FirstOrDefault().AddressId;
        }

       
        public async Task AddCard(CardModel card)
        {
            var existCard = ListCard().FirstOrDefault(i => i.Number == card.CardNumber);
            var lines = new List<string>();

            if (existCard == null)
            {
                var creditCard = new CreditCard()
                {
                    Number = card.CardNumber,
                    Name = card.Name,
                    ShelfLife = card.ShelfLife,
                    Cvv = card.CVV,
                    Cpf = SessionId
                };

                await contexts.CreditCard.AddAsync(creditCard);
                await contexts.SaveChangesAsync();
            }
        }

        public int IdInserted()
        {
            return ListCard().OrderByDescending(j => j.CreditCardId).FirstOrDefault().CreditCardId;
        }

        
        public string ClientName(string cpf)
        {
            return GetUser().Name;
        }

        
        public async Task<bool> HasCard()
        {
            return await contexts.CreditCard.AnyAsync(i => i.Cpf == SessionId);
        }

       
        public async Task<bool> HasAddress()
        {
            return await contexts.Address.AnyAsync(i => i.Cpf == SessionId);
        }

        
        public async Task<List<ShowCardModel>> ShowCards()
        {
            return await context.CreditCard
                .Where(i => i.Cpf == SessionId)
                .Select(i => new ShowCardModel
                {
                    CardNumber = i.Number.ToString(),
                    Name = i.Name,
                    ShelfLife = i.ShelfLife
                })
                .ToListAsync();
        }

        
        public async Task<List<ShowAddressModel>> ShowAddress()
        {
            var client = GetUser();
            var listAddress = ListAddress();

            return await context.Address
                .Where(i => i.Cpf == SessionId)
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
                .ToListAsync();
        }
    }
}
