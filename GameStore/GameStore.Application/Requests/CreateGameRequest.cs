using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;

namespace GameStore.Application.Requests
{
    public class CreateGameRequest
    {
        public required GameDTO Game { get; set; }

        public required ICollection<GenreDTO> Genres{ get; set; }
        public required ICollection<PlatformDTO> Platforms { get; set; }
    }
}
