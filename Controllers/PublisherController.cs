using Microsoft.AspNetCore.Mvc;
using EpicGameWebAppStore.DataAccess; // Đảm bảo rằng dòng này có mặt
using EpicGameWebAppStore.Domain.Entities;
using System.Linq;
using EpicGameWebAppStore.Infrastructure.DataAccess;

namespace EpicGameWebAppStore.Controllers
{
    public class PublisherController : Controller
    {
        private readonly EpicgamewebappContext _context;

        public PublisherController(EpicgamewebappContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var publishers = _context.Publishers.ToList();
            return View(publishers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                // Không gán giá trị cho PublisherID, để MySQL tự động tăng
                _context.Publishers.Add(publisher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public IActionResult Edit(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        [HttpPost]
        public IActionResult Edit(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Publishers.Update(publisher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public IActionResult Delete(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
