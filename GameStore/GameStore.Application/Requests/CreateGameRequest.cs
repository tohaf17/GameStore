using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;

namespace GameStore.Application.Requests
{
    public class CreateGameRequest
    {
        public required CreateGameDto Game { get; set; }

        public required ICollection<GenreDto> Genres{ get; set; }
        public required ICollection<PlatformDto> Platforms { get; set; }
    }
}
