using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using EpicGameWebAppStore.Models;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublisherController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    // Get all publishers
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllPublishers()
    {
        var publishers = await _publisherService.GetAllPublishers();
        return Ok(publishers);
    }

    // Get publisher by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPublisherById(int id)
    {
        var publisher = await _publisherService.GetPublisherById(id);
        if (publisher == null)
        {
            return NotFound("Publisher not found.");
        }
        return Ok(publisher);
    }

    // Get publishers by name
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetPublishersByName(string name)
    {
        var publishers = await _publisherService.GetPublisherByName(name);
        return Ok(publishers);
    }

    // Get publishers by address
    [HttpGet("address/{address}")]
    public async Task<IActionResult> GetPublishersByAddress(string address)
    {
        var publishers = await _publisherService.GetPublisherByAddress(address);
        return Ok(publishers);
    }

    // Add a new publisher
    [HttpPost("createpublisher")]
    public async Task<IActionResult> AddPublisher([FromBody] Publisher publisher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newPublisher = await _publisherService.AddPublisher(publisher);
        return CreatedAtAction(nameof(GetPublisherById), new { id = newPublisher.PublisherId }, newPublisher);
    }

    // Update an existing publisher
    [HttpPut("UpdatePublisher/{id}")]
    public async Task<IActionResult> UpdatePublisher(int id, [FromBody] PublisherFormModel publishermodel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
               var publisher = new Publisher
{
    PublisherId = id,
    Name = publishermodel.Name,
    Address = publishermodel.Address,
    Email = publishermodel.Email,
    Phone = publishermodel.Phone,
    Website = publishermodel.Website,
};
        try
        {
            var updatedPublisher = await _publisherService.UpdatePublisher(publisher);
            return Ok(updatedPublisher);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Delete a publisher
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublisher(int id)
    {
        try
        {
            var deletedPublisher = await _publisherService.DeletePublisher(id);
            return Ok(deletedPublisher);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
