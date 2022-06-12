using GuessMyNumber.Models;
using System.Security.Claims;

namespace GuessMyNumber.Services
{
    /// <summary>
    /// Game service interface
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Gets ClaimsPrincipal class using a specific claims identities
        /// </summary>
        /// <param name="gameId">The unique game ID</param>
        /// <returns>ClaimsPrincipal</returns>
        ClaimsPrincipal? GenerateToken(string gameId);

        /// <summary>
        /// Gets created new game
        /// </summary>
        /// <returns>New game</returns>
        Game CreateNewGame();

        /// <summary>
        /// Gets guess result with try count and one of game rusult: win, too big, too low
        /// </summary>
        /// <param name="guess">Guess number</param>
        /// <param name="gameId">The unique game ID</param>
        /// <returns>Guess result</returns>
        /// <exception cref="NullReferenceException"></exception>
        GuessResult Guess(int guess, string gameId);

        /// <summary>
        /// Gets best games
        /// </summary>
        /// <param name="number">Number of best games to take</param>
        /// <returns>List of games</returns>
        public IEnumerable<IGame> GetBestGames(int number);
    }
}