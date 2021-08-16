using Microsoft.AspNetCore.Mvc;
using SingleExperience.Domain.Enums;
using SingleExperience.Repository.Services.BoughtServices;
using SingleExperience.Repository.Services.BoughtServices.Models;
using SingleExperience.Repository.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleExperience.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoughtController : ControllerBase
    {
        protected readonly BoughtService bought;

        public BoughtController(BoughtService bought) => this.bought = bought;


        // GET: api/<BoughtController>
        [HttpGet]
        public async Task<PreviewBoughtModel> Preview([FromBody] BuyModel boughtModel, int addressId)
        {
            return await bought.PreviewBoughts(boughtModel, addressId);
        }

        // GET: api/<BoughtController>
        [HttpGet]
        public async Task<List<BoughtModel>> Show()
        {
            return await bought.Show();
        }

        // GET: api/<BoughtController>
        [HttpGet]
        public async Task<List<BoughtModel>> ListAll()
        {
            return await bought.ListAll();
        }

        // GET: api/<BoughtController>
        [HttpGet]
        public async Task<List<BoughtModel>> Status(StatusBoughtEnum status)
        {
            return await bought.BoughtPendent(status);
        }

        // GET: api/<BoughtController>
        [HttpGet]
        public async Task<bool> Exist(int boughtId)
        {
            return await bought.Exist(boughtId);
        }

        // POST api/<BoughtController>
        [HttpPost]
        public async Task Add([FromBody] AddBoughtModel addBought)
        {
            await bought.Add(addBought);
        }

        // PUT api/<BoughtController>/5
        [HttpPut("{id}")]
        public async Task UdateStatus(int boughtId, StatusBoughtEnum status)
        {
            await bought.UpdateStatus(boughtId, status);
        }
    }
}
