using GuessMyNumber.Models;

namespace GuessMyNumber.Services
{
    public interface IGameRepository
    {
        void Add(Game game);

        public Game? GetGameById(string gameId);

        public IEnumerable<IGame> GetBestGames(int number);
    }
}