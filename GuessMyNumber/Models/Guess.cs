using System.ComponentModel.DataAnnotations;

namespace GuessMyNumber.Models
{
    /// <summary>
    /// Guess class
    /// </summary>
    public class Guess
    {
        [Required]
        [Range(1, 100)]
        public int Number { get; set; }
    }
}
