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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net;
using System.Threading.Tasks;
using webapi.DTO;
using webapi.Models;

namespace webapi.DBConextion
{
    public class FakeDBMethods : IDBMethods
    {
        private List<Mission> missions = new List<Mission>() {
            new Mission{ ID = 1,MissionName="M1"},
            new Mission{ ID = 2,MissionName="M2"},
            new Mission{ ID = 3,MissionName="M3"},
            new Mission{ ID = 4,MissionName="M4"},           
            new Mission{ ID = 5,MissionName="M5"},           
            new Mission{ ID = 6,MissionName="M6"},
        };
        private List<Rocket> rockets = new List<Rocket>() {
            new Rocket{ ID =1, RateSucessRocket=23, RocketName="rocket1" },
            new Rocket{ ID =2, RateSucessRocket=35,RocketName= "rocket2" },
            new Rocket{ ID =3, RateSucessRocket=13,RocketName= "rocket3" },
            new Rocket{ID = 4, RateSucessRocket=3, RocketName="rocket4" }
        };
        private List<Launch> launches = new List<Launch>() {
             new Launch  { ID =1,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 1,MissionName="M1"} ,  Rocket=new Rocket{ ID =1, RateSucessRocket=23, RocketName="rocket1" } ,  FirstRocketlaunch=true },
             new Launch  { ID =2,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 2,MissionName="M2"} ,  Rocket=new Rocket{ ID =2, RateSucessRocket=35, RocketName="rocket2" } ,  FirstRocketlaunch=true },
             new Launch  { ID =3,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 3,MissionName="M3"} ,  Rocket=new Rocket{ ID =3, RateSucessRocket=13, RocketName="rocket3" } ,  FirstRocketlaunch=true },
             new Launch  { ID =4,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 4,MissionName="M4"} ,  Rocket=new Rocket{ ID =4, RateSucessRocket=3, RocketName="rocket4" } ,  FirstRocketlaunch=true },
             new Launch  { ID =5,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 5,MissionName="M5"} ,  Rocket=new Rocket{ ID =1, RateSucessRocket=23, RocketName="rocket1" } ,  FirstRocketlaunch=true },
             new Launch  { ID =6,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 6,MissionName="M6"} ,  Rocket=new Rocket{ ID =2, RateSucessRocket=35, RocketName="rocket2" } ,  FirstRocketlaunch=true },
             new Launch  { ID =7,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 1,MissionName="M1"} ,  Rocket=new Rocket{ ID =3, RateSucessRocket=13, RocketName="rocket3" } ,  FirstRocketlaunch=false },
             new Launch  { ID =8,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 2,MissionName="M2"} ,  Rocket=new Rocket{ ID =4, RateSucessRocket=3, RocketName="rocket4" } ,  FirstRocketlaunch=false },
            new Launch  { ID =9,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 1,MissionName="M1"} ,  Rocket=new Rocket{ ID =1, RateSucessRocket=23, RocketName="rocket1" } ,  FirstRocketlaunch=true },
             new Launch  { ID =10,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 2,MissionName="M2"} ,  Rocket=new Rocket{ ID =2, RateSucessRocket=35, RocketName="rocket2" } ,  FirstRocketlaunch=true },
             new Launch  { ID =11,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 3,MissionName="M3"} ,  Rocket=new Rocket{ ID =3, RateSucessRocket=13, RocketName="rocket3" } ,  FirstRocketlaunch=true },
             new Launch  { ID =12,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 3,MissionName="M3"} ,  Rocket=new Rocket{ ID =3, RateSucessRocket=13, RocketName="rocket3" } ,  FirstRocketlaunch=true },
             new Launch  { ID =13,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 3,MissionName="M3"} ,  Rocket=new Rocket{ ID =3, RateSucessRocket=13, RocketName="rocket3" } ,  FirstRocketlaunch=true },
             new Launch  { ID =14,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 4,MissionName="M4"} ,  Rocket=new Rocket{ ID =4, RateSucessRocket=3, RocketName="rocket4" } ,  FirstRocketlaunch=true },
             new Launch  { ID =15,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 5,MissionName="M5"} ,  Rocket=new Rocket{ ID =1, RateSucessRocket=23, RocketName="rocket1" } ,  FirstRocketlaunch=true },
             new Launch  { ID =16,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 6,MissionName="M6"} ,  Rocket=new Rocket{ ID =2, RateSucessRocket=35, RocketName="rocket2" } ,  FirstRocketlaunch=true },
             new Launch  { ID =17,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 1,MissionName="M1"} ,  Rocket=new Rocket{ ID =3, RateSucessRocket=13, RocketName="rocket3" } ,  FirstRocketlaunch=false },
             new Launch  { ID =18,  DateLunch=DateTime.Now, Mission=new Mission{ ID = 2,MissionName="M2"} ,  Rocket=new Rocket{ ID =4, RateSucessRocket=3, RocketName="rocket4" } ,  FirstRocketlaunch=false },
       }; 
                  
        public async Task<List<MissionDTO>> Get(DbtestContext DbtestContext, int from, int per_page)
        {
            var launch = launches.Select(
                s => new MissionDTO
                {
                    MissionName = s.Mission != null ? s.Mission.MissionName : ""

                }
            ).Skip(from).Take(per_page).ToList();


            return launch;

        }

        public async Task<LaunchDTO> GetById(MemoryCacheEntryOptions _cacheEntryOptions, DbtestContext DbtestContext, IMemoryCache cache, int id)
        {
            LaunchIdDTO? launch = launches.Select
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
                  ).FirstOrDefault(s => s.ID == id);

            if (launch == null)   ///not found
            {
                return null;
            }
            else
            {
                LaunchDTO result = new LaunchDTO    /// found 
                {
                    DateCached = launch.DateCached,
                    MissionName = launch.MissionName,
                    DateLunch = launch.DateLunch,
                    RocketName = launch.RocketName,
                    FirstRocketlaunch = launch.FirstRocketlaunch,
                    RateSucessRocket = launch.RateSucessRocket
                };
                Console.WriteLine("Adding to Cache...");
                cache.Set(id, result, _cacheEntryOptions);

                return result;
            }


        }
        public async Task<HttpStatusCode> InsertLaunch(DbtestContext DbtestContext, LaunchInsertDTO Launch)
        {
            var mission = missions.FirstOrDefault(s => s.ID == Launch.MissionID);

            var rocket = rockets.FirstOrDefault(s => s.ID == Launch.RocketID);
            if (rocket == null || mission == null)
            {
                return HttpStatusCode.NotFound;
            }
            var entity = new Launch()
            {
                MissionID = Launch.MissionID,
                DateLunch = Launch.DateLunch,
                RocketID = Launch.RocketID,
                Rocket = rocket,
                Mission = mission,
                FirstRocketlaunch = Launch.FirstRocketlaunch
            };

            launches.Add(entity);            

            return HttpStatusCode.Created;
        }
        public async Task<HttpStatusCode> DeleteLaunch(DbtestContext DbtestContext, IMemoryCache cache, int Id)
        {
            var entity = launches.Find(s => s.ID == Id);
            if (entity == null)
            {
                return HttpStatusCode.NotFound;
            }
            launches.Remove(entity);

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
            var mission = missions.Select(
                s => new Mission
                {
                    ID = s.ID,
                    MissionName = s.MissionName
                }
            ).ToList();


            return mission;

        }
        public async Task<List<Rocket>> GetRockets(DbtestContext DbtestContext)   //list of rockets
        {
            var rocket = rockets.Select(
                s => new Rocket
                {
                    ID = s.ID,
                    RocketName = s.RocketName,
                    RateSucessRocket = s.RateSucessRocket

                }
            ).ToList();

            return rocket;


        }
    }
}
