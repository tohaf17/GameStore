using GameStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Models.Requests
{
    public class CreateGameRequest
    {
        public GameDTO Game {  get; set; }
        public ICollection<Guid> Genres { get; set; }
        public ICollection<Guid> Platforms { get; set;  }
    }
}
