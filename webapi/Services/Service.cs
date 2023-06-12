using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using webapi.DBConextion;
using webapi.DTO;
using webapi.Models;
namespace webapi.Services
{
    public class Service
    {
        private readonly IDBMethods idbMethods;
        private readonly DbtestContext DbtestContext;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;
        private readonly IMemoryCache cache;
        public Service(IDBMethods idbMethods, DbtestContext DbtestContext, IMemoryCache _cache)
        {
            this.idbMethods = idbMethods;
            this.DbtestContext = DbtestContext;
            _cacheEntryOptions = GetCacheEntryOptions();
            this.cache = _cache;
        }        
        private static MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(45))
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSize(100);
        }
        [HttpGet(Name = "per_page={x}&page={y}")]
        public async Task<ActionResult<List<MissionDTO>>> Get(string? perpage, string? currentpage)
        {
            //calculate o pagination values
            int per_page;
            if (perpage == null || !int.TryParse(perpage, out per_page))
            {
                per_page = 10;
            }
            else
            {
                if (per_page <= 0)
                {
                    per_page = 10;
                }
            }
            int page;
            if (currentpage == null || !int.TryParse(currentpage, out page))
            {
                page = 1;
            }
            else
            {
                if (page <= 0)
                {
                    page = 1;
                }
            }
            int from = ((page - 1) * per_page);
            int to = per_page * page;            
            var launch = await idbMethods.Get(DbtestContext, from, per_page);
            if (launch.Count < 0)
            {
                return null;
            }
            else
            {
                return launch;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LaunchDTO>> GetById(int id)
        {
            if (!cache.TryGetValue(id, out LaunchDTO element))///item not cached
            {
                LaunchDTO result = await idbMethods.GetById(_cacheEntryOptions, DbtestContext, cache, id);
                if (result == null)
                {
                    return null;
                }
                return result;
            }
            else
            {
                Console.WriteLine("Reading of Cache...");
                return new LaunchDTO
                {
                    DateCached = element.DateCached,
                    MissionName = element.MissionName,
                    DateLunch = element.DateLunch,
                    RocketName = element.RocketName,
                    FirstRocketlaunch = element.FirstRocketlaunch,
                    RateSucessRocket = element.RateSucessRocket
                };
            }
        }
        [HttpPost("InsertLaunch")]
        public async Task<HttpStatusCode> InsertLaunch(LaunchInsertDTO Launch)
        {
            return await idbMethods.InsertLaunch(DbtestContext, Launch);            
        }

        [HttpDelete("DeleteLaunch/{Id}")]
        public async Task<HttpStatusCode> DeleteLaunch(int Id)
        {
            return await idbMethods.DeleteLaunch(DbtestContext, cache, Id);
        }

        [HttpGet("missions")]
        public async Task<ActionResult<List<Mission>>> GetMissions() //list of missions
        {
            return await idbMethods.GetMissions(DbtestContext);

        }
        [HttpGet("rockets")]
        public async Task<ActionResult<List<Rocket>>> GetRockets()   //list of rockets
        {
            return await idbMethods.GetRockets(DbtestContext); 
            
        }
    }
}
