using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpicGameWebAppStore.Controllers;

    [Route("[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: api/Discount
        [HttpGet("GetAllDiscount")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts()
        {
            try
            {
                var discounts = await _discountService.GetAllDiscountAsync();
                return Ok(discounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Discount/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Discount>> GetDiscountById(int id)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);
                if (discount == null)
                    return NotFound($"Discount with ID {id} not found.");
                return Ok(discount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Discount
        [HttpPost]
        public async Task<ActionResult<Discount>> AddDiscount([FromBody] Discount discount)
        {
            if (discount == null)
            {
                return BadRequest("Discount data is null");
            }

            try
            {
                var createdDiscount = await _discountService.AddDiscountAsync(discount);
                return CreatedAtAction(nameof(GetDiscountById), new { id = createdDiscount.DiscountId }, createdDiscount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Discount/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Discount>> UpdateDiscount(int id, [FromBody] Discount discount)
        {
            if (id != discount.DiscountId)
            {
                return BadRequest("Discount ID mismatch");
            }

            try
            {
                var updatedDiscount = await _discountService.UpdateDiscountAsync(discount);
                return Ok(updatedDiscount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Discount/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Discount>> DeleteDiscount(int id)
        {
            try
            {
                var deletedDiscount = await _discountService.DeleteDiscountAsync(id);
                return Ok(deletedDiscount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

