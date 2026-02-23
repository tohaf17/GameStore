using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Game
    {
        [Required]
        public Guid Id { get; set; }
        [Required] 
        public string Name { get;set; }=string.Empty;
        [Required]
        public string Key { get; set; }=string.Empty;
        public string? Description { get; set; }=string.Empty;

        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
        public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
    }
}
