using System.ComponentModel.DataAnnotations;

namespace GuessMyNumber.Models
{
    /// <summary>
    /// Game class
    /// </summary>
    public class Game : IGame
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        [Required]
        public int NumberToGuess { get; set; }

        public int TryCount { get; set; }

        public TimeSpan PlayTime => EndDateTime - StartDateTime;

        public string GetId => Id.ToString();

        public bool IsPlaying => StartDateTime >= EndDateTime;

        /// <summary>
        /// Gets new instance of game class
        /// </summary>
        /// <param name="numberToGuess">Number to guess</param>
        /// <returns>Game</returns>
        public static Game CreateGame(int numberToGuess)
        {
            return new Game()
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTime.Now,
                NumberToGuess = numberToGuess,
                TryCount = 0
            };
        }
    }
}
