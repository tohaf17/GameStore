using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class GenreDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid? ParentGenreId { get; set; }

    }
}
