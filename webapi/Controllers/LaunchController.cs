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

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class LaunchesController : ControllerBase
{

    private readonly DbtestContext DbtestContext;
    private readonly ILogger<LaunchesController> _logger;
     private readonly MemoryCacheEntryOptions _cacheEntryOptions;
     private readonly IMemoryCache cache ;

    public LaunchesController(ILogger<LaunchesController> logger, DbtestContext DbtestContext, IMemoryCache _cache)
    {
        _logger = logger;
        this.DbtestContext = DbtestContext;
        _cacheEntryOptions = GetCacheEntryOptions();
       // cache = new MemoryCache(new MemoryCacheOptions());
        this.cache = _cache;

    }
    private static MemoryCacheEntryOptions GetCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions()
            //.SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
            //.SetSlidingExpiration(TimeSpan.FromSeconds(3600))
            .SetAbsoluteExpiration(TimeSpan.FromDays(45))
            .SetPriority(CacheItemPriority.NeverRemove)
            .SetSize(100);
    }
    [HttpGet(Name = "per_page={x}&page={y}")]
    public async Task<ActionResult<List<MissionDTO>>> Get(string? perpage, string? currentpage)
    {
        int per_page;
        if (perpage == null || !int.TryParse(perpage, out per_page))
        {
            per_page = 10;

        }
        else { 
            if(per_page <= 0){
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
        
        var launch = await DbtestContext.Launches.Select(
            s => new MissionDTO
            {
                 MissionName= s.Mission != null? s.Mission.MissionName :""
                 
            }
        ).Skip(from).Take(per_page).ToListAsync();


        if (launch.Count < 0)
        {
            return NotFound();
        }
        else
        {
            return launch;
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<LaunchDTO>> GetById(int id)
    {

 
        if (!cache.TryGetValue(id,out LaunchDTO element))///item not cached
        {
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

            if (launch==null)   ///not found in DB
            {

                return NotFound();
            }
            else
            {      
                LaunchDTO result = new LaunchDTO
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
        else
        {
            //LaunchDTO? launch = element;
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
            Rocket=rocket,
            Mission=mission,
            FirstRocketlaunch = Launch.FirstRocketlaunch
         };

        DbtestContext.Launches.Add(entity);
        await DbtestContext.SaveChangesAsync();

        return HttpStatusCode.Created;
    }

   
    [HttpDelete("DeleteLaunch/{Id}")]
    public async Task<HttpStatusCode> DeleteLaunch(int Id)
    {
        var entity = new Launch()
        {
            ID = Id
        };
        DbtestContext.Launches.Attach(entity);
        DbtestContext.Launches.Remove(entity);
        await DbtestContext.SaveChangesAsync();

         ///delete of cache space
        Console.WriteLine("Removing of Cache...");
        cache.Remove(Id);

        return HttpStatusCode.OK;
    }

    [HttpGet("missions")]
    public async Task<ActionResult<List<Mission>>> GetMissions()
    {
        var mission = await DbtestContext.Missions.Select(
            s => new Mission
            {
                ID= s.ID,
                MissionName= s.MissionName
            }
        ).ToListAsync();

        if (mission.Count < 0)
        {
            return NotFound();
        }
        else
        {
            return mission;
        }
    }
    [HttpGet("rockets")]
    public async Task<ActionResult<List<Rocket>>> GetRockets()
    {
        var rocket = await DbtestContext.Rockets.Select(
            s => new Rocket
            {
                ID = s.ID,
                RocketName = s.RocketName,
                RateSucessRocket=s.RateSucessRocket

            }
        ).ToListAsync();


        if (rocket.Count < 0)
        {
            return NotFound();
        }
        else
        {
            return rocket;
        }

    }
}