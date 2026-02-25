using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Key { get; set; }
        public required string Description { get; set; }
    }
}
