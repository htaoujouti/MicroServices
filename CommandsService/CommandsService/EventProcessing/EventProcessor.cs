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
        

    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
