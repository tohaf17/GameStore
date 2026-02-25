using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class PlatformDto
    {
        public Guid Id { get; set; }
        public required string Type { get; set; }
    }
}
