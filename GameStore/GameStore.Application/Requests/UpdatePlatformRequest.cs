using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdatePlatformRequest
    {
        public PlatformDTO Platform { get; set; }
    }
}
