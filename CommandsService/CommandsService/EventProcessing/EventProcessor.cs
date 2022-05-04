using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopefactory;

        public IMapper _mapper { get; }

        public EventProcessor(IServiceScopeFactory scopefactory, IMapper mapper)
        {
            _scopefactory = scopefactory;
            _mapper = mapper;

        }
        public void ProcessEvent(string message)
        {

        }
        

    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
