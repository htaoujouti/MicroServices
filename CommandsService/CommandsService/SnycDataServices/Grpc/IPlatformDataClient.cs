using CommandsService.Models;
using System.Collections.Generic;

namespace CommandsService.SnycDataServices.Grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}
