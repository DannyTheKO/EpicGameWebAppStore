using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers
{
    public class _BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
