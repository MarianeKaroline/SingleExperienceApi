using Microsoft.AspNetCore.Mvc;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Services.ClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleExperience.WebAPI.Controllers
{
    [Route("singleexperience")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        protected readonly ClientService client;

        public ClientController(ClientService client) => this.client = client;


        // GET singleexperience/creditcard
        [HttpGet("creditcard")]
        public async Task<List<ShowCardModel>> ShowCard()
        {
            return await client.ShowCards();
        }

        // GET singleexperience/address
        [HttpGet("address")]
        public async Task<List<ShowAddressModel>> ShowAddress()
        {
            return await client.ShowAddress();
        }

        // GET singleexperience/hascard
        [HttpGet("hascard")]
        public async Task<bool> HasCard()
        {
            return await client.HasCard();
        }

        // GET singleexperience/hasaddress
        [HttpGet("hasaddress")]
        public async Task<bool> HasAddress()
        {
            return await client.HasAddress();
        }

        // POST: singleexperience/addcard
        [HttpPost("addcard")]
        public async Task AddCard([FromBody] CardModel card)
        {
            await client.AddCard(card);
        }

        // POST: singleexperience/addaddress
        [HttpPost("addaddress")]
        public async Task<int> AddAddress([FromBody] AddressModel address)
        {
            return await client.AddAddress(address);
        }

        // POST: singleexperience/signup
        [HttpPost("signup")]
        public async Task<bool> SignUp([FromBody] SignUpModel signUp)
        {
            return await client.SignUpClient(signUp);
        }
    }
}
