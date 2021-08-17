using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Services.ClientServices;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        protected readonly ClientService client;

        public ClientController(ClientService client) => this.client = client;

        [HttpGet("creditcard")]
        public async Task<List<ShowCardModel>> ShowCards()
        {
            return await client.ShowCards();
        }

        [HttpGet("address")]
        public async Task<List<ShowAddressModel>> ShowAddresses()
        {
            return await client.ShowAddresses();
        }

        [HttpGet("has/card")]
        public async Task<bool> ExistCard()
        {
            return await client.ExistCard();
        }

        [HttpGet("has/address")]
        public async Task<bool> ExistAddress()
        {
            return await client.ExistAddress();
        }

        [HttpPost("card")]
        public async Task AddCard([FromBody] CardModel card)
        {
            await client.AddCard(card);
        }

        [HttpPost("address")]
        public async Task<int> AddAddress([FromBody] AddressModel address)
        {
            return await client.AddAddress(address);
        }

        [HttpPost("signup")]
        public async Task<bool> Signup([FromBody] SignUpModel signUp)
        {
            return await client.Signup(signUp);
        }
    }
}
