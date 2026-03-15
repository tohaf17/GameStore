using System.ComponentModel.DataAnnotations;
namespace GameStore.Domain.Entities
{
    public class Platform
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Type { get; set; }

        public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
    }
}
