using GuessMyNumber.Enums;

namespace GuessMyNumber.Models
{
    public class GuessResult
    {
        public int TryCount { get; set; }

        public GameResult GameResult { get; set; }
    }
}
