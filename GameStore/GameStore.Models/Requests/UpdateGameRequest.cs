using GameStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Models;

namespace GameStore.Models.Requests
{
    public class UpdateGameRequest
    {
        public Game Game { get; set; }
        public ICollection<Guid> Genres { get; set; }
        public ICollection<Guid> Platforms { get; set; }
    }
}
