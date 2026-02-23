using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class UpdateGameDTO
    {
        public Guid Id
        {
            get; set;
        }
        public string Key { get; set; }
        public required string Name { get; set; }
        public Guid ParentGenreId { get; set; }
    }
}
