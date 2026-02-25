using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdateGenreRequest
    {
        public required GenreDto Genre { get; set; }

    }
}
