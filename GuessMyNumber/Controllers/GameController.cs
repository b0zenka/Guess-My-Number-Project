using Microsoft.AspNetCore.Mvc;

namespace GuessMyNumber.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Start()
        {
            return View();
        }
    }
}
