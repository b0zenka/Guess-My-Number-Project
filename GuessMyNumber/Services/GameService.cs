using GuessMyNumber.Enums;
using GuessMyNumber.Models;
using System.Security.Claims;

namespace GuessMyNumber.Services
{
    /// <summary>
    /// Game service class
    /// </summary>
    public class GameService : IGameService
    {
        private const int MIN_VALUE = 1;
        private const int MAX_VALUE = 100;

        private readonly IGameRepository gameRepository;
        private readonly Random random;

        /// <summary>
        /// Initilialize a new instance of game servise class
        /// </summary>
        /// <param name="gameRepository">Game repository</param>
        public GameService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
            this.random = new Random();
        }

        /// <summary>
        /// Gets created new game
        /// </summary>
        /// <returns>New game</returns>
        public Game CreateNewGame()
        {
            int numberToGuess = random.Next(MIN_VALUE, MAX_VALUE + 1);
            var game = Game.CreateGame(numberToGuess);

            gameRepository.Add(game);

            return game;
        }

        /// <summary>
        /// Gets ClaimsPrincipal class using a specific claims identities
        /// </summary>
        /// <param name="gameId">The unique game ID</param>
        /// <returns>ClaimsPrincipal</returns>
        public ClaimsPrincipal? GenerateToken(string gameId)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, gameId)
            };

            var gameIdentity = new ClaimsIdentity(claims, "Game Identity");
            var principal = new ClaimsPrincipal(new[] { gameIdentity });

            return principal;
        }

        /// <summary>
        /// Gets guess result with try count and one of game rusult: win, too big, too low
        /// </summary>
        /// <param name="guess">Guess number</param>
        /// <param name="gameId">The unique game ID</param>
        /// <returns>Guess result</returns>
        /// <exception cref="NullReferenceException"></exception>
        public GuessResult Guess(int guess, string gameId)
        {
            Game? game = gameRepository.GetGameById(gameId);

            if (game is null)
                throw new NullReferenceException("Game not found");

            GameResult gameResult = GetGameResult(game.NumberToGuess, guess);

            if (gameResult == GameResult.Winner)
                SetEndTime(game);

            GuessResult guessResult = new GuessResult()
            {
                GameResult = gameResult,
                TryCount = ++game.TryCount
            };

            return guessResult;
        }

        /// <summary>
        /// Gets best games
        /// </summary>
        /// <param name="number">Number of best games to take</param>
        /// <returns>List of games</returns>
        public IEnumerable<IGame> GetBestGames(int number)
        {
            return gameRepository.GetBestGames(number);
        }

        /// <summary>
        /// Gets games result: win, too big or too low
        /// </summary>
        /// <param name="numberToGuess">Number to guess</param>
        /// <param name="guess">Guess number</param>
        /// <returns>Game result</returns>
        private GameResult GetGameResult(int numberToGuess, int guess)
        {
            if (guess > numberToGuess)
                return GameResult.TooBig;
            else if (guess < numberToGuess)
                return GameResult.TooLow;
            else
                return GameResult.Winner;
        }

        /// <summary>
        /// Sets end time of game
        /// </summary>
        /// <param name="game">Game</param>
        private void SetEndTime(Game game)
        {
            game.EndDateTime = DateTime.Now;
        }
    }
}
