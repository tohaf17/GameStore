using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class GameGenre
    {
        [Required]
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        [Required]
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}
