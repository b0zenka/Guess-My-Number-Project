using GuessMyNumber.Models;

namespace GuessMyNumber.Services
{
    public class GameRepository : IGameRepository
    {
        private List<Game> games;

        public GameRepository()
        {
            games = new List<Game>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public void Add(Game game)
        {
            games.Add(game);
        }

        public Game? GetGameById(string gameId)
        {
            return games.FirstOrDefault(x => x.Id.ToString() == gameId);
        }

        public IEnumerable<IGame> GetBestGames(int gamesCount)
        {
            return games.Where(x => !x.IsPlaying)
                .OrderBy(x => x.TryCount)
                .ThenBy(x => x.PlayTime)
                .Take(gamesCount);
        }
    }
}
