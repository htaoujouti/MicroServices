﻿using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDtos>> GetPlatforms()
        {
            Console.WriteLine("GetPlatforms() from Command service");
            var PlatformItem = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(PlatformItem));
        }
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDtos> GetPlatformById(int id)
        {
            Console.WriteLine("GetPlatformById(id)");
            var platform = _repo.GetPlatformById(id);
            if (platform != null)
                return Ok(_mapper.Map<PlatformReadDtos>(platform));
            return NotFound();
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound post # command service");
            return Ok("Inbound test of from Platforms Controller");

        }
    }
}
