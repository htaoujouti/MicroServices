﻿using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDtos>();
            CreateMap<Command , CommandReadDtos>();
            CreateMap<CommandCreateDtos, Command>();
        }
    }
}
