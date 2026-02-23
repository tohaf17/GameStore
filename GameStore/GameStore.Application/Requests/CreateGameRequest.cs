using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;

namespace GameStore.Application.Requests
{
    public class CreateGameRequest
    {
        public GameDTO Game { get; set; }

        public ICollection<GenreDTO> Genres{ get; set; }
        public ICollection<PlatformDTO> Platforms { get; set; }
    }
}
