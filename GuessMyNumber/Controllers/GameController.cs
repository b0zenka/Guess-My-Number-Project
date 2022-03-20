using GuessMyNumber.Models;
using GuessMyNumber.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuessMyNumber.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService service;

        public GameController(IGameService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Start()
        {
            IGame game = service.CreateNewGame();
            ViewData["token"] = service.GenerateToken(game.GetId);
            return View();
        }
    }
}
