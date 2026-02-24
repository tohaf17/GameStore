using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdateGameRequest
    {
        public GameDTO Game { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
        public ICollection<PlatformDTO> Platforms { get; set; }
    }
}
