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

<<<<<<< HEAD
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
=======
	// GET: Publisher/Index
	public async Task<IActionResult> Index()
	{
		var publishers = await _publisherService.GetAllPublishers();
		return View(publishers);
	}
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677

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

<<<<<<< HEAD
        // == Add a Publisher ==
        [HttpPost]
        public async Task<IActionResult> AddPublisher([FromBody] Publisher publisher)
        {
            try
            {
                if (publisher == null) return BadRequest("Invalid publisher data.");
=======
	// POST: Publisher/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Publisher publisher)
	{
		if (ModelState.IsValid)
		{
			await _publisherService.AddPublisher(publisher);
			TempData["Message"] = "Publisher created successfully.";
			return RedirectToAction(nameof(Index));
		}
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677

                var createdPublisher = await _publisherService.AddPublisherAsync(publisher);
                return CreatedAtAction(nameof(GetPublisherById), new { id = createdPublisher.PublisherId }, createdPublisher);

<<<<<<< HEAD
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
=======
	// GET: Publisher/Edit/{id}
	public async Task<IActionResult> Edit(int id)
	{
		var publisher = await _publisherService.GetPublisherById(id);
		if (publisher == null) return NotFound();
		return View(publisher);
	}
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677

        // == Update a Publisher ==
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] Publisher publisher)
        {
            try
            {
                if (publisher == null || publisher.PublisherId != id) return BadRequest("Publisher ID mismatch.");

<<<<<<< HEAD
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
=======
		if (ModelState.IsValid)
		{
			await _publisherService.UpdatePublisher(publisher);
			TempData["Message"] = "Publisher updated successfully.";
			return RedirectToAction(nameof(Index));
		}

		return View(publisher);
	}

	// GET: Publisher/Delete/{id}
	public async Task<IActionResult> Delete(int id)
	{
		var publisher = await _publisherService.GetPublisherById(id);
		if (publisher == null) return NotFound();
		return View(publisher);
	}

	// POST: Publisher/Delete/{id}
	[HttpPost]
	[ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _publisherService.DeletePublisher(id);
		TempData["Message"] = "Publisher deleted successfully.";
		return RedirectToAction(nameof(Index));
	}
}
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677
