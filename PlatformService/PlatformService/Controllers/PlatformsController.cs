using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SnycDataServices.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _mapper = mapper;
            _repo = repo;
            _commandDataClient = commandDataClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("GetPlatforms()");
            var PlatformItem = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(PlatformItem));
        }
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("GetPlatformById(id)");
            var platform = _repo.GetPlatformById(id);
            if (platform != null)
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto dto)
        {
            var platform = _mapper.Map<Platform>(dto);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            } catch (Exception ex)
            {
                Console.WriteLine($"--> Could not Send syncronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}
