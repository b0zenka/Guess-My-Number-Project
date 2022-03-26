using GuessMyNumber.Models;
using System.Security.Claims;

namespace GuessMyNumber.Services
{
    public interface IGameService
    {
        ClaimsPrincipal? GenerateToken(string gameId);

        Game CreateNewGame();

        GuessResult Guess(int guess, string gameId);

        public IEnumerable<IGame> GetBestGames(int number);
    }
}