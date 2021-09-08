using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Services.CartServices;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        protected readonly CartService cart;

        public CartController(CartService cart) => this.cart = cart;

        [HttpGet("{sessionId}")]
        public async Task<List<ProductCartModel>> ShowProducts(string sessionId)
        {           
            return await cart.ShowProducts(sessionId);
        }

        [HttpGet("total/{sessionId}")]
        public async Task<TotalCartModel> Total(string sessionId)
        {
            return await cart.Total(sessionId);
        }

        [HttpGet("edit/status/{sessionId}")]
        public async Task<bool> EditStatus([FromBody] List<BuyProductModel> products, string sessionId)
        {
            return await cart.CallEditStatus(products, sessionId);
        }

        [HttpPost("{productId}/{sessionId}")]
        public async Task<bool> AddProduct(int productId, string sessionId)
        {
            return await cart.AddProduct(productId, sessionId);
        }

        [HttpDelete("{productId}/{sessionId}")]
        public async Task RemoveProduct(int productId, string sessionId)
        {
            await cart.RemoveProduct(productId, sessionId);
        }
    }
}
