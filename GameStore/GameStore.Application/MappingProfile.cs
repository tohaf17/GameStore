using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GameStore.Domain.Entities;
using GameStore.Application.DTO;

namespace GameStore.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, CreateGameDto>().ReverseMap();
            CreateMap<Game, GameDTO>().ReverseMap();
            CreateMap<Genre,CreateGenreDto>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Platform, CreatePlatformDto>().ReverseMap();
            CreateMap<Platform, PlatformDTO>().ReverseMap();
        }
    }
}
