using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Services.UserServices;
using SingleExperience.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SingleExperience.Domain;
using System.Threading.Tasks;
using System.Linq;

namespace SingleExperience.Services.ClientServices
{
    public class ClientService : UserService
    {
        protected readonly Context contexts;

        public ClientService(Context context) : base(context)
        {
            contexts = context;
        }
       
        public List<Address> GetAddress()
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
        
        public List<CreditCard> GetCard()
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
        
        public async Task<List<ShowAddressModel>> ShowAddresses()
        {
            var client = GetUser();
            var listAddress = GetAddress();

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

        public async Task<bool> ExistCard()
        {
            return await contexts.CreditCard.AnyAsync(i => i.Cpf == SessionId);
        }
       
        public async Task<bool> ExistAddress()
        {
            return await contexts.Address.AnyAsync(i => i.Cpf == SessionId);
        }

        public int IdInserted()
        {
            return GetCard().OrderByDescending(j => j.CreditCardId).FirstOrDefault().CreditCardId;
        }                
        
        public async Task<int> AddAddress(AddressModel addressModel)
        {
            var address = new Address()
            {
                PostCode = addressModel.Postcode,
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
            var existCard = GetCard().FirstOrDefault(i => i.Number == card.CardNumber);
            var lines = new List<string>();

            if (existCard == null)
            {
                var creditCard = new CreditCard()
                {
                    Number = card.CardNumber,
                    Name = card.Name,
                    ShelfLife = card.ShelfLife,
                    Cvv = card.Cvv,
                    Cpf = SessionId
                };

                await contexts.CreditCard.AddAsync(creditCard);
                await contexts.SaveChangesAsync();
            }
        }

        public async Task<bool> Signup(SignUpModel client)
        {
            client.Validator();

            var existClient = await context.Enjoyer
                .Where(i => i.Cpf == client.Cpf)
                .FirstOrDefaultAsync();

            if (existClient == null)
            {
                await SignUp(client);
            }

            return existClient == null;
        }        
    }        
}
