/*using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace webapi.DBConextion
{
    public class CacheManage
    {
        private const string Key = "MyKey";
        private static readonly Random Random = new Random();
        private static MemoryCacheEntryOptions _cacheEntryOptions;
        public static void Main()
        {
            _cacheEntryOptions = GetCacheEntryOptions();
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            SetKey(cache, "0");
            PeriodicallyReadKey(cache, TimeSpan.FromSeconds(1));
            PeriodicallyRemoveKey(cache, TimeSpan.FromSeconds(11));
            PeriodicallySetKey(cache, TimeSpan.FromSeconds(13));
            Console.ReadLine();
            Console.WriteLine("Shutting down");
        }
        private static void SetKey(IMemoryCache cache, string value)
        {
            Console.WriteLine("Setting: " + value);
            cache.Set(Key, value, _cacheEntryOptions);
        }
        private static MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(7))
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .RegisterPostEvictionCallback(AfterEvicted, state: null);
        }
        private static void AfterEvicted(object key, object value, EvictionReason reason, object state)
        {
            Console.WriteLine("Evicted. Value: " + value + ", Reason: " + reason);
        }
        private static void PeriodicallySetKey(IMemoryCache cache, TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);
                    SetKey(cache, "A");
                }
            });
        }
        private static void PeriodicallyReadKey(IMemoryCache cache, TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);
                    if (Random.Next(3) == 0)
                    {
                        Console.WriteLine("Read skipped, random choice.");
                    }
                    else
                    {
                        Console.Write("Reading...");
                        object result;
                        if (!cache.TryGetValue(Key, out result))
                        {
                            result = cache.Set(Key, "B", _cacheEntryOptions);
                        }
                        Console.WriteLine("Read: " + (result ?? "(null)"));
                    }
                }
            });
        }
        private static void PeriodicallyRemoveKey(IMemoryCache cache, TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);
                    Console.WriteLine("Removing...");
                    cache.Remove(Key);
                }
            });
        }

}*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using webapi.DBConextion;
using webapi.DTO;
using webapi.Models;

namespace webapi.DBConextion
{
    public class DBMethods : IDBMethods
    {

        public async Task<List<MissionDTO>> Get(DbtestContext DbtestContext,int from, int per_page)
        {
            //request to data base
            var launch = await DbtestContext.Launches.Select(
                s => new MissionDTO
                {
                    MissionName = s.Mission != null ? s.Mission.MissionName : ""

                }
            ).Skip(from).Take(per_page).ToListAsync();


                return launch;
            
        }
    
        public async Task<LaunchDTO> GetById(MemoryCacheEntryOptions cacheEntryOptions,DbtestContext DbtestContext, IMemoryCache cache,int id)
        {
            //request to data base
            LaunchIdDTO? launch = await DbtestContext.Launches.Select
                   (
                       s => new LaunchIdDTO
                       {
                           ID = s.ID,
                           DateCached = DateTime.Now,
                           MissionName = s.Mission != null ? s.Mission.MissionName : "",
                           DateLunch = s.DateLunch,
                           RocketName = s.Rocket != null ? s.Rocket.RocketName : "",
                           FirstRocketlaunch = s.FirstRocketlaunch,
                           RateSucessRocket = s.Rocket != null ? s.Rocket.RateSucessRocket : 0
                       }
                   ).FirstOrDefaultAsync(s => s.ID == id);

                if (launch == null)   ///not found in DB
                {

                    return null;
                }
                else
                {
                    LaunchDTO result = new LaunchDTO    /// found in DB
                    {
                        DateCached = launch.DateCached,
                        MissionName = launch.MissionName,
                        DateLunch = launch.DateLunch,
                        RocketName = launch.RocketName,
                        FirstRocketlaunch = launch.FirstRocketlaunch,
                        RateSucessRocket = launch.RateSucessRocket
                    };
                    Console.WriteLine("Adding to Cache...");
                    cache.Set(id, result, cacheEntryOptions);

                    return result;
                }
           

        }
        public async Task<HttpStatusCode> InsertLaunch(DbtestContext DbtestContext,LaunchInsertDTO Launch)
        {
            var mission = await DbtestContext.Missions.FirstOrDefaultAsync(s => s.ID == Launch.MissionID);

            var rocket = await DbtestContext.Rockets.FirstOrDefaultAsync(s => s.ID == Launch.RocketID);
            if (rocket == null || mission == null)
            {
                return HttpStatusCode.NotFound;
            }
            var entity = new Launch()
            {
                //DateCached = Launch.DateCached,
                MissionID = Launch.MissionID,
                DateLunch = Launch.DateLunch,
                RocketID = Launch.RocketID,
                Rocket = rocket,
                Mission = mission,
                FirstRocketlaunch = Launch.FirstRocketlaunch
            };

            DbtestContext.Launches.Add(entity);
            await DbtestContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }
         public async Task<HttpStatusCode> DeleteLaunch(DbtestContext DbtestContext, IMemoryCache cache, int Id)
        {
            var entity = new Launch()
            {
                ID = Id
            };
            DbtestContext.Launches.Attach(entity);
            DbtestContext.Launches.Remove(entity);
            await DbtestContext.SaveChangesAsync();

            ///delete of cache space
            if (cache.TryGetValue(Id, out LaunchDTO element))
            {
                Console.WriteLine("Removing of Cache...");
                cache.Remove(Id);

            }

            return HttpStatusCode.OK;
        }
         public async Task<List<Mission>> GetMissions(DbtestContext DbtestContext) //list of missions
        {
            var mission = await DbtestContext.Missions.Select(
                s => new Mission
                {
                    ID = s.ID,
                    MissionName = s.MissionName
                }
            ).ToListAsync();

            
                return mission;
             
        }
         public async Task<List<Rocket>> GetRockets(DbtestContext DbtestContext)   //list of rockets
        {
            var rocket = await DbtestContext.Rockets.Select(
                s => new Rocket
                {
                    ID = s.ID,
                    RocketName = s.RocketName,
                    RateSucessRocket = s.RateSucessRocket

                }
            ).ToListAsync();
  
                return rocket;
            

        }
    }
}
