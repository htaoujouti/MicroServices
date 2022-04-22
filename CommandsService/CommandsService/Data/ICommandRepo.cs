using CommandsService.Models;
using System.Collections.Generic;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool saveChanges();

        //platforms
        IEnumerable<Platform> GetAllPlatforms();
        bool PlatformExist(int platformId);
        void CreatePlatform(Platform platform);

        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId,int commandId);
        void CreateCommand(int platformId, Command command);
    }
}
