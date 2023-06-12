using Microsoft.Extensions.Caching.Memory;
using System.Net;
using webapi.DTO;
using webapi.Models;

namespace webapi.DBConextion
{
    public interface IDBMethods
    {
         Task<List<MissionDTO>> Get(DbtestContext DbtestContext, int from, int per_page);
          Task<LaunchDTO> GetById(MemoryCacheEntryOptions _cacheEntryOptions, DbtestContext DbtestContext, IMemoryCache _cache, int id);
          Task<HttpStatusCode> InsertLaunch(DbtestContext DbtestContext, LaunchInsertDTO Launch);
          Task<HttpStatusCode> DeleteLaunch(DbtestContext DbtestContext, IMemoryCache _cache, int Id);
          Task<List<Mission>> GetMissions(DbtestContext DbtestContext);
          Task<List<Rocket>> GetRockets(DbtestContext DbtestContext);
    }
}