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
    }
}
