using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDtos>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");
            if (!_repo.PlatformExist(platformId))
            {
                return NotFound();
            }
            var Commands = _repo.GetCommandsForPlatform(platformId);          
            return Ok(_mapper.Map<IEnumerable<CommandReadDtos>>(Commands));                      
        }
        [HttpGet("{id}", Name = "GetCommandsForPlatform")]
        public ActionResult<CommandReadDtos> GetCommandsForPlatform(int platformId, int id)
        {
            Console.WriteLine($"--> GetCommandsForPlatform {id}:  {platformId}");
            if (!_repo.PlatformExist(platformId))
            {
                return NotFound();
            }
            var Commands = _repo.GetCommand(platformId, id);
            if (Commands == null)               
                return NotFound();
            return Ok(_mapper.Map<CommandReadDtos>(Commands));
        }
        [HttpPost]
        public ActionResult<CommandReadDtos> CreateCommandForPlatform(int platformId, CommandCreateDtos dto)
        {
            Console.WriteLine($"--> CreateCommandForPlatform:  {platformId}");
            if (!_repo.PlatformExist(platformId))
            {
                return NotFound();
            }
            var command = _mapper.Map<Command>(dto);
            _repo.CreateCommand(platformId, command);
            _repo.saveChanges();

            var commandReadDto = _mapper.Map<CommandReadDtos>(command);
           return CreatedAtRoute(nameof(CreateCommandForPlatform),
               new {platformId= commandReadDto.PlatformId, Id = commandReadDto.Id}, commandReadDto);

        }
    }
}
