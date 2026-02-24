using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class CreateGameDTO
    {
        public string Key { get; set; }
        public required string Name { get; set; }
        public Guid ParentGenreId { get; set; }
    }
}
