using GuessMyNumber.Enums;

namespace GuessMyNumber.Models
{
    /// <summary>
    /// Guess result class
    /// </summary>
    public class GuessResult
    {
        public int TryCount { get; set; }

        public GameResult GameResult { get; set; }
    }
}
