using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdateGameRequest
    {
        public required GameDto Game { get; set; }
        public required ICollection<GenreDto> Genres { get; set; }
        public required ICollection<PlatformDto> Platforms { get; set; }
    }
}
