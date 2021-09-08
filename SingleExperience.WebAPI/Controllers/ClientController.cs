using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Services.ClientServices;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        protected readonly ClientService client;

        public ClientController(ClientService client) => this.client = client;

        [HttpGet("creditcard/{sessionId}")]
        public async Task<List<ShowCardModel>> ShowCards(string sessionId)
        {
            return await client.ShowCards(sessionId);
        }

        [HttpGet("address/{sessionId}")]
        public async Task<List<ShowAddressModel>> ShowAddresses(string sessionId)
        {
            return await client.ShowAddresses(sessionId);
        }

        [HttpPost("creditcard")]
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
