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

        [HttpGet("manager/allboughts")]
        public async Task<List<BoughtModel>> GetAll()
        {
            return await bought.GetAll();
        }

        [HttpGet("preview")]
        public async Task<PreviewBoughtModel> Preview([FromBody] BuyModel boughtModel)
        {
            return await bought.Preview(boughtModel);
        }

        [HttpGet("user/boughts")]
        public async Task<List<BoughtModel>> Show()
        {
            return await bought.Show();
        }

        [HttpGet("manager/boughts/{status}")]
        public async Task<List<BoughtModel>> Status(StatusBoughtEnum status)
        {
            return await bought.Status(status);
        }

        [HttpGet("exist-bought/{boughtId}")]
        public async Task<bool> Exist(int boughtId)
        {
            return await bought.Exist(boughtId);
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
