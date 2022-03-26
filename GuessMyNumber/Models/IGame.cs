
namespace GuessMyNumber.Models
{
    public interface IGame
    {
        int NumberToGuess { get; set; }
        string GetId { get; }
        TimeSpan PlayTime { get; }
        int TryCount { get; set; }
        bool IsPlaying { get; }
    }
}