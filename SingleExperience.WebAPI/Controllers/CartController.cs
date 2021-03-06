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

        [HttpGet]
        public async Task<List<ProductCartModel>> ShowProducts()
        {
            return await cart.ShowProducts();
        }

        [HttpGet("total")]
        public async Task<TotalCartModel> Total()
        {
            return await cart.Total();
        }

        [HttpGet("edit-status")]
        public async Task<bool> EditStatus([FromBody] List<BuyProductModel> products)
        {
            return await cart.CallEditStatus(products);
        }

        [HttpPost("addcart")]
        public async Task AddProduct([FromBody] CartModel cartModel)
        {
            await cart.AddProduct(cartModel);
        }

        [HttpPost("passcart")]
        public async Task PassProducts()
        {
            await cart.PassProducts();
        }

        [HttpPut("removeproduct/{productId}")]
        public async Task RemoveProduct(int productId)
        {
            await cart.RemoveProduct(productId);
        }
    }
}
