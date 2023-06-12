using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using webapi.DBConextion;
using webapi.DTO;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class LaunchesController : ControllerBase
{

    private readonly Service service;
    public LaunchesController(DbtestContext DbtestContext, IMemoryCache _cache)
    {
        
        this.service = new Service(new DBMethods(), DbtestContext, _cache);
    }
     
    [HttpGet(Name = "per_page={x}&page={y}")]
    public async Task<ActionResult<List<MissionDTO>>> Get(string? perpage, string? page)
    {
         var result = await service.Get(perpage, page);
        if (result == null) { return NotFound(); }
        return result;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<LaunchDTO>> GetById(int id)
    {
        var result = await service.GetById(id);
        if(result == null) {return  NotFound(); }
        return result;   
       
    }

    [HttpPost("InsertLaunch")]
    public async Task<HttpStatusCode> InsertLaunch(LaunchInsertDTO Launch)
    {
         return await service.InsertLaunch(Launch);

    }

    [HttpDelete("DeleteLaunch/{Id}")]
    public async Task<HttpStatusCode> DeleteLaunch(int Id)
    {
            return await service.DeleteLaunch(Id); 
    }

    [HttpGet("missions")]
    public async Task<ActionResult<List<Mission>>> GetMissions() //list of missions
    {
            return await service.GetMissions();      
    }
    [HttpGet("rockets")]
    public async Task<ActionResult<List<Rocket>>> GetRockets()   //list of rockets
    {
            return await service.GetRockets();       

    }
}