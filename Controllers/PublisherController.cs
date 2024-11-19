using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        // == Get All Publishers ==
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPublishers()
        {
            try
            {
                var publishers = await _publisherService.GetAllPublishersAsync();
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // == Get Publisher by ID ==
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            try
            {
                var publisher = await _publisherService.GetPublisherByIdAsync(id);
                if (publisher == null) return NotFound("Publisher not found.");
                return Ok(publisher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // == Add a Publisher ==
        [HttpPost]
        public async Task<IActionResult> AddPublisher([FromBody] Publisher publisher)
        {
            try
            {
                if (publisher == null) return BadRequest("Invalid publisher data.");

                var createdPublisher = await _publisherService.AddPublisherAsync(publisher);
                return CreatedAtAction(nameof(GetPublisherById), new { id = createdPublisher.PublisherId }, createdPublisher);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // == Update a Publisher ==
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] Publisher publisher)
        {
            try
            {
                if (publisher == null || publisher.PublisherId != id) return BadRequest("Publisher ID mismatch.");

                var updatedPublisher = await _publisherService.UpdatePublisherAsync(publisher);
                return Ok(updatedPublisher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // == Delete a Publisher ==
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                var deletedPublisher = await _publisherService.DeletePublisherAsync(id);
                return Ok(deletedPublisher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
