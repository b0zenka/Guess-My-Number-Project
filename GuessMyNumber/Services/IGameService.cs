using GuessMyNumber.Models;

namespace GuessMyNumber.Services
{
    public interface IGameService
    {
        string GenerateToken(string gameId);

        Game CreateNewGame();
    }
}