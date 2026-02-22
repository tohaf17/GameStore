using System.ComponentModel.DataAnnotations;
namespace GameStore.Models
{
    public class Platform
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Type { get; set; }

        public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
    }
}
