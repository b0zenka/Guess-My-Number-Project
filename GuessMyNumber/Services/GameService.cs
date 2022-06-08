using GuessMyNumber.Enums;
using GuessMyNumber.Models;
using System.Security.Claims;

namespace GuessMyNumber.Services
{
    public class GameService : IGameService
    {
        private const int MIN_VALUE = 1;
        private const int MAX_VALUE = 100;

        private readonly IGameRepository gameRepository;
        private readonly Random random;

        public GameService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
            this.random = new Random();
        }

        public Game CreateNewGame()
        {
            int numberToGuess = random.Next(MIN_VALUE, MAX_VALUE + 1);
            var game = Game.CreateGame(numberToGuess);

            gameRepository.Add(game);

            return game;
        }

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

        public IEnumerable<IGame> GetBestGames(int number)
        {
            return gameRepository.GetBestGames(number);
        }

        private GameResult GetGameResult(int numberToGuess, int guess)
        {
            if (guess > numberToGuess)
                return GameResult.TooBig;
            else if (guess < numberToGuess)
                return GameResult.TooLow;
            else
                return GameResult.Winner;
        }

        private void SetEndTime(Game game)
        {
            game.EndDateTime = DateTime.Now;
        }
    }
}
