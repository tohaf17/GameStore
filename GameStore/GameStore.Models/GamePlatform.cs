using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class GamePlatform
    {
        [Required]
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        [Required]
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;
    }
}
