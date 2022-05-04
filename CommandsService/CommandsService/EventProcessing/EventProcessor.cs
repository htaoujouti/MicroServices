using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
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
            var eventType = Determinate(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;
                default:
                    break;
            }
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
        private void addPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopefactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);

                    if (!repo.ExternalPlatformExist(plat.ExternalId)){
                        repo.CreatePlatform(plat);
                        repo.saveChanges();
                        Console.WriteLine("--> platform added!");
                    }
                    else
                    {
                        Console.WriteLine("--> platform already exsits...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB: {ex.Message}");
                }
            }
        }

    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
