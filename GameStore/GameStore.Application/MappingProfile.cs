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
            CreateMap<Game, GameDto>().ReverseMap();
            CreateMap<Genre,CreateGenreDto>().ReverseMap();
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<Platform, CreatePlatformDto>().ReverseMap();
            CreateMap<Platform, PlatformDto>().ReverseMap();
        }
    }
}
