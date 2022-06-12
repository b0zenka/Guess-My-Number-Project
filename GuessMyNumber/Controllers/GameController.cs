using GuessMyNumber.Enums;
using GuessMyNumber.Models;
using GuessMyNumber.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuessMyNumber.Controllers
{
    /// <summary>
    /// Game controller
    /// </summary>
    public class GameController : Controller
    {
        private readonly IGameService service;

        /// <summary>
        /// Initilialize a new instance of game controller
        /// </summary>
        /// <param name="service">Game service</param>
        public GameController(IGameService service)
        {
            this.service = service;
        }

        /// GET
        /// <summary>
        /// Gets start view
        /// </summary>
        /// <returns>Start view</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// GET: /Game/Guess
        /// <summary>
        /// Initialize new game and genarete token with game Id and set it into cookie
        /// </summary>
        /// <returns>Guess view</returns>
        [HttpGet]
        public async Task<IActionResult> Guess()
        {
            IGame game = service.CreateNewGame();
            var principal = service.GenerateToken(game.GetId);

            await HttpContext.SignInAsync(principal);
            ViewBag.GuessResult = new GuessResult() { GameResult = GameResult.None, TryCount = 0 };

            return View();
        }

        /// POST: /Game/Guess
        /// <summary>
        /// Endpoint responsible for guessing the number for the game
        /// </summary>
        /// <remarks>
        /// Gets game ID from cookies and check guess numer for that game
        /// </remarks>
        /// <param name="guess">Guess number</param>
        /// <returns>Guess result</returns>
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

        /// GET: /Game/Highscores
        /// <summary>
        /// Gets high scores games
        /// </summary>
        /// <returns>View with best games</returns>
        [HttpGet]
        public IActionResult HighScores()
        {
            int bestResultCount = 10;
            var games = service.GetBestGames(bestResultCount);

            return View(games);
        }
    }
}
