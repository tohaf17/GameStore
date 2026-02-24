using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class CreateGenreDTO
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
