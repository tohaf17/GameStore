using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class PlatformDTO
    {
        public Guid Id { get; set; }
        public required string Type { get; set; }
    }
}
