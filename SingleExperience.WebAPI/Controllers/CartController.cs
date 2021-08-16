using Microsoft.AspNetCore.Mvc;
using SingleExperience.Domain.Entities;
using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Services.CartServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleExperience.WebAPI.Controllers
{
    [Route("singleexperience/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        protected readonly CartService cart;

        public CartController(CartService cart) => this.cart = cart;

        // GET singleexperience/cart/
        [HttpGet]
        public async Task<List<ProductCartModel>> ShowProducts()
        {
            return await cart.ShowProducts();
        }

        // GET singleexperience/cart/total
        [HttpGet("total")]
        public async Task<TotalCartModel> Total()
        {
            return await cart.Total();
        }

        // GET singleexperience/cart/edit-status
        [HttpGet("edit-status")]
        public async Task<bool> EditStatus([FromBody] List<BuyProductModel> products)
        {
            return await cart.CallEditStatus(products);
        }

        // POST singleexperience/addcart
        [HttpPost("addcart")]
        public async Task Add([FromBody] CartModel cartModel)
        {
            await cart.AddProduct(cartModel);
        }

        // POST singleexperience/addcart
        [HttpPost("passcart")]
        public async Task Pass()
        {
            await cart.PassProducts();
        }

        // PUT singleexperience/removeProduct/5
        [HttpPut("removeproduct/{productId}")]
        public async Task RemoveItem(int productId)
        {
            await cart.RemoveItem(productId);
        }
    }
}
