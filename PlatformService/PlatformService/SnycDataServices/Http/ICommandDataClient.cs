using PlatformService.Dtos;
using System.Threading.Tasks;

namespace PlatformService.SnycDataServices.Http
{
    public interface ICommandDataClient 
    {
        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}
