using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameStore.Models.DTO
{
    public class GameDTO
    {
        public string Name { get; set; } = string.Empty;
        
        public string Key { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
