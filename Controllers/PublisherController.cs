using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

public class PublisherController : Controller
{
	private readonly IPublisherService _publisherService;

	public PublisherController(
		IPublisherService publisherService)
	{
		_publisherService = publisherService;
	}

	// GET: Publisher/Index
	public async Task<IActionResult> Index()
	{
		var publishers = await _publisherService.GetAllPublishers();
		return View(publishers);
	}

	// GET: Publisher/Create
	public IActionResult Create()
	{
		return View();
	}

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

		return View(publisher);
	}

	// GET: Publisher/Edit/{id}
	public async Task<IActionResult> Edit(int id)
	{
		var publisher = await _publisherService.GetPublisherById(id);
		if (publisher == null) return NotFound();
		return View(publisher);
	}

	// POST: Publisher/Edit/{id}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, Publisher publisher)
	{
		if (id != publisher.PublisherId) return BadRequest();

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