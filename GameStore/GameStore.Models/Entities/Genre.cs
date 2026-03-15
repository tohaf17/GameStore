using System.ComponentModel.DataAnnotations;
namespace GameStore.Domain.Entities
{
    public class Genre
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid? ParentGenreId { get; set; }
        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    }
}
