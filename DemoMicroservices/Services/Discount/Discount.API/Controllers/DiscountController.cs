using Discount.API.Entities;
using Discount.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscount _discount;

        public DiscountController(IDiscount discount)
        {
            _discount = discount;
        }

        [HttpGet("{productName}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetProduct(string productName)
        {
            var coupon = await _discount.GetDiscount(productName);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> CreateDiscount([FromBody] Coupon coupon)
        {
            await _discount.CreateDiscount(coupon);
            return CreatedAtRoute("GetProduct", new { ProductName=coupon.ProductName}, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
        {
            
            return Ok(await _discount.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            return Ok(await _discount.DeleteDiscount(productName));
        }
    }
}
