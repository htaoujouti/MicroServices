using CommandsService.Models;
using CommandsService.SnycDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandsService.Data
{
    public class PrepDb
    {
        public static void PrePopulation(IApplicationBuilder builder)
        {
            using ( var serviceScoped = builder.ApplicationServices.CreateScope())
            {
                var clientGrpc = serviceScoped.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = clientGrpc.ReturnAllPlatforms();
                SeedData(serviceScoped.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> seed new platforms...");
            foreach(Platform platform in platforms)
            {
                if(!repo.ExternalPlatformExist(platform.ExternalId))
                {
                    repo.CreatePlatform(platform);
                }
                repo.saveChanges();
            }
        }
    }
}
