using System.ComponentModel.DataAnnotations;

namespace GuessMyNumber.Models
{
    public class Guess
    {
        [Required]
        [Range(1, 100)]
        public int Number { get; set; }
    }
}
