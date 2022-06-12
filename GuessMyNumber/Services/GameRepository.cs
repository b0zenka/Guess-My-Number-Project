using GuessMyNumber.Models;

namespace GuessMyNumber.Services
{
    /// <summary>
    /// Game repository class
    /// </summary>
    public class GameRepository : IGameRepository
    {
        private List<Game> games;

        /// <summary>
        /// Initialize a new instance of game repository class
        /// </summary>
        public GameRepository()
        {
            games = new List<Game>();
        }

        /// <summary>
        /// Adds game to game repository list
        /// </summary>
        /// <param name="game">Game to be added to game repository list</param>
        public void Add(Game game)
        {
            games.Add(game);
        }

        /// <summary>
        /// Gets game associated with the specified ID
        /// </summary>
        /// <param name="gameId">The unique game ID</param>
        /// <returns>Game</returns>
        public Game? GetGameById(string gameId)
        {
            return games.FirstOrDefault(x => x.Id.ToString() == gameId);
        }

        /// <summary>
        /// Gets best games
        /// </summary>
        /// <param name="gamesCount">Number of best games to take</param>
        /// <returns>List of games</returns>
        public IEnumerable<IGame> GetBestGames(int gamesCount)
        {
            return games.Where(x => !x.IsPlaying)
                .OrderBy(x => x.TryCount)
                .ThenBy(x => x.PlayTime)
                .Take(gamesCount);
        }
    }
}
