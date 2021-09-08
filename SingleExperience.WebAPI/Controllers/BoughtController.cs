using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Repository.Services.BoughtServices;
using SingleExperience.Domain.Enums;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoughtController : ControllerBase
    {
        protected readonly BoughtService bought;

        public BoughtController(BoughtService bought) => this.bought = bought;

        [HttpGet("allboughts")]
        public async Task<List<BoughtModel>> GetAll()
        {
            return await bought.GetAll();
        }

        [HttpGet("preview")]
        public async Task<PreviewBoughtModel> Preview([FromBody] BuyModel boughtModel)
        {
            return await bought.Preview(boughtModel);
        }

        [HttpGet("boughts/{sessionId}")]
        public async Task<List<BoughtModel>> Show(string sessionId)
        {
            return await bought.Show(sessionId);
        }

        [HttpGet("{status}")]
        public async Task<List<BoughtModel>> GetStatus(StatusBoughtEnum status)
        {
            return await bought.GetStatus(status);
        }

        [HttpPost("addbought")]
        public async Task Add([FromBody] AddBoughtModel addBought)
        {
            await bought.Add(addBought);
        }

        [HttpPut("{boughtId}/{status}")]
        public async Task UdateStatus(int boughtId, StatusBoughtEnum status)
        {
            await bought.UpdateStatus(boughtId, status);
        }
    }
}
