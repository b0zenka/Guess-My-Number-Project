using GuessMyNumber.Enums;
using GuessMyNumber.Models;
using GuessMyNumber.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Guess()
        {
            IGame game = service.CreateNewGame();
            var principal = service.GenerateToken(game.GetId);

            await HttpContext.SignInAsync(principal);
            ViewBag.GuessResult = new GuessResult() { GameResult = GameResult.None, TryCount = 0 };

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Guess([FromHeader]Guess guess)
        {
            string gameId = HttpContext.User
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;

            var result = service.Guess(guess.Number, gameId);

            ViewBag.GuessResult = result;
            return View();
        }

        [HttpGet]
        public IActionResult HighScores()
        {
            int bestResultCount = 10;
            var games = service.GetBestGames(bestResultCount);

            return View(games);
        }
    }
}
