using GuessMyNumber.Models;

namespace GuessMyNumber.Services
{
    /// <summary>
    /// Game repository interface
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Adds game to game repository list
        /// </summary>
        /// <param name="game">Game to be added to game repository list</param>
        void Add(Game game);

        /// <summary>
        /// Gets game associated with the specified ID
        /// </summary>
        /// <param name="gameId">The unique game ID</param>
        /// <returns>Game</returns>
        public Game? GetGameById(string gameId);

        /// <summary>
        /// Gets best games
        /// </summary>
        /// <param name="gamesCount">Number of best games to take</param>
        /// <returns>List of games</returns>
        public IEnumerable<IGame> GetBestGames(int number);
    }
}