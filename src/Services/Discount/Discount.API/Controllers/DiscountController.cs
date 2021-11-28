using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            this._discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        }

        #region Actions
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> Get(int id, CancellationToken cancellationToken)
        {
            var coupon = await this._discountRepository.GetCouponAsync(id, cancellationToken);
            if (coupon is null)
                return NotFound();

            return Ok(coupon);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> Get([Required] string productId, CancellationToken cancellationToken)
        {
            var coupon = await this._discountRepository.GetCouponAsync(productId, cancellationToken);
            if (coupon is null)
                return NotFound();

            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromBody] Coupon coupon, CancellationToken cancellationToken)
        {
            bool success = await this._discountRepository.CreateCouponAsync(coupon, cancellationToken);
            return success ?
                CreatedAtAction(nameof(Get), new { coupon.ProductId }, null) :
                BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] Coupon coupon, CancellationToken cancellationToken)
        {
            coupon.Id = id;
            bool success = await this._discountRepository.UpdateCouponAsync(coupon, cancellationToken);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            bool success = await this._discountRepository.DeleteCouponAsync(id, cancellationToken);
            return success ? Ok() : NotFound();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string productId, CancellationToken cancellationToken)
        {
            bool success = await this._discountRepository.DeleteCouponAsync(productId, cancellationToken);
            return success ? Ok() : NotFound();
        }

        #endregion
    }
}
