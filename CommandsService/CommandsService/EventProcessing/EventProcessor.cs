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
        private EventType Determinate(string message)
        {
            Console.WriteLine("--> Determining Event...");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);
            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> platform Published Event Detected!");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type!");
                    return EventType.Undetermined;
            }
        }
        

    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
