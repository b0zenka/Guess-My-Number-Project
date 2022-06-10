
using System.ComponentModel.DataAnnotations;

namespace GuessMyNumber.Models
{
    public interface IGame
    {
        [Display(Name = "Zgadywana liczba")]
        int NumberToGuess { get; set; }

        [Display(Name = "Identyfikator")]
        string GetId { get; }

        [Display(Name = "Czas gry")]
        TimeSpan PlayTime { get; }

        [Display(Name = "Liczba prób")]
        int TryCount { get; set; }

        bool IsPlaying { get; }
    }
}