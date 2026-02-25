using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdateGameRequest
    {
        public required GameDTO Game { get; set; }
        public required ICollection<GenreDTO> Genres { get; set; }
        public required ICollection<PlatformDTO> Platforms { get; set; }
    }
}
