using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;
using webapi.Controllers;
using webapi.DBConextion;
using webapi.DTO;
using webapi.Models;
using webapi.Services;

namespace webapi.Test;

public class LaunchTest
{
    private Service service;
    [SetUp]
    public void Setup()
    {
         this.service = new Service(new FakeDBMethods(), new DbtestContext(), new MemoryCache(new MemoryCacheOptions()));
    }

    // [TestCase()]\
    [Test]
    public async Task GetLaunches_Whith_ParamsOK()
    {
        //pagination, per page and page are valid values 
        string perpage = "3";
        string page = "3";
        var result = await service.Get(perpage, page);
        Assert.AreEqual(3, result.Value.Count,null, "solution 3 elements in page 3");

         perpage = "3";
        page = "20";
        result = await service.Get(perpage, page);
        Assert.AreEqual(0, result.Value.Count, null, "solution 0 elements in page 20");

    }
    [Test]
    public async Task GetLaunches_Whith_PerPage_Number_Invalid()
    {
        //pagination, per page is not a valid number, the method Get take by default per page=10 
        string perpage = "string";
        string page = "1";
        var result = await service.Get(perpage, page);
        Assert.AreEqual(10, result.Value.Count, null, "solution 10 elements in page 1");
        
    }
    [Test]
    public async Task GetLaunches_Whith_PerPage_Number_Less_Than_Zero()
    {
        //pagination, per page is less than 0, the method Get take by default per page=10 
        string perpage = "-1";
        string page = "1";
        var result = await service.Get(perpage, page);
        Assert.AreEqual(10, result.Value.Count, null, "solution 10 elements in page 1");
    }
    [Test]
    public async Task GetLaunches_Whith_PerPage_Number_Equal_Zero()
    {
        //pagination, per page is equal 0, the method Get take by default per page=10 
        string perpage = "0";
        string page = "1";
        var result = await service.Get(perpage, page);
        Assert.AreEqual(10, result.Value.Count, null, "solution 10 elements in page 1");
    }
    [Test]
    public async Task GetLaunches_Whith_PerPage_Null()
    {
        //pagination, per page is null, the method Get take by default per page=10 
        string page = "1";
        var result = await service.Get(null, page);
        Assert.AreEqual(10, result.Value.Count, null, "solution 10 elements in page 1");
    }

    [Test]
    public async Task GetLaunches_Whith_page_Number_Invalid()
    {
        //pagination,  page is not  a valid number, the method Get take by default  page=1 
        string perpage = "5";
        string page = "string";
        var result = await service.Get(perpage, page);
        Assert.AreEqual(5, result.Value.Count, null, "solution 10 elements in page 1");

    }
    [Test]
    public async Task GetLaunches_Whith_page_Number_Less_Than_Zero()
    {
        //pagination,  page is less than 0, the method Get take by default page=1 
        string perpage = "5";
        string page = "-1";
        var result = await service.Get(perpage, page);
        Assert.AreEqual(5, result.Value.Count, null, "solution 10 elements in page 1");
    }
    [Test]
    public async Task GetLaunches_Whith_Page_Number_Equal_Zero()
    {
        //pagination,   page is equal 0, the method Get take by default   page=1 
        string perpage = "5";
        string page = "0";
        var result = await service.Get(perpage, page);
        Assert.AreEqual(5, result.Value.Count, null, "solution 10 elements in page 1");
    }
    [Test]
    public async Task GetLaunches_Whith_page_Null()
    {
        //pagination,   page is null, the method Get take by default   page=1 
        string perpage = "5";
        var result = await service.Get(perpage, null);
        Assert.AreEqual(5, result.Value.Count, null, "solution 10 elements in page 1");
    }

    [Test]
    public async Task GetByIDLaunches_Whith_id_Correct()
    {
        //id is in the fakeData 
        int id = 5;
        var result = await service.GetById(id);
        
        Assert.AreEqual("M5", result.Value.MissionName, null, "object found");
    }
    [Test]
    public async Task GetByIDLaunches_Whith_id_inValid()
    {
        //id is not in the fakeData  or not valid
        int id = 55;
        var result = await service.GetById(id);
         Assert.AreEqual(null, result, null, "object not found");
    }
    [Test]
    public async Task GetByIDLaunches_Whith_id_in_Cache()
    {
        //id is not in the fakeData 
        IMemoryCache cache= new MemoryCache(new MemoryCacheOptions());
        MemoryCacheEntryOptions _cacheEntryOptions=new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(45))
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSize(100);
        int id = 22;   
        LaunchDTO launchElement = new LaunchDTO
        {
            DateCached = DateTime.Now,
            MissionName = "Mission Cache",
            DateLunch = DateTime.Now,
            RocketName = "Rocket Cache",
            FirstRocketlaunch = true,
            RateSucessRocket = 33
        };
        cache.Set(id, launchElement, _cacheEntryOptions);  //insert data in cache 
        Service servicecache = new Service(new FakeDBMethods(), new DbtestContext(), cache);

        var result = await servicecache.GetById(id);  //id is the same inserted in chache
         Assert.AreEqual("Mission Cache", result.Value.MissionName, null, "object cached and the method didn`t make the request to the fake data it is showing the chache data");
    }

    [Test]
    public async Task GetByIDLaunches_Whith_id_Get_With_Cache()
    {
         
        IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(45))
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSize(100);       
        
        Service servicecache = new Service(new FakeDBMethods(), new DbtestContext(), cache);
        var result = await servicecache.GetById(5);  
         Assert.AreEqual(cache.Get(5), result.Value, null, "found the object and saved it in cache ");
    }
    [Test]
    public async Task Insert_Launches()
    {
        string perpage = "300";
        string page = "1";
        var result = await service.Get(perpage, page);
        var count = result.Value.Count;
        LaunchInsertDTO launchElement = new LaunchInsertDTO
        {
            DateLunch = DateTime.Now,
            MissionID = 1,
            RocketID = 2,
            FirstRocketlaunch = true
        };

        var results = await service.InsertLaunch(launchElement);
        result = await service.Get(perpage, page);
        var countinsert = result.Value.Count;
        Assert.AreEqual(count+1, countinsert, null, "inserted the object ");
    }
    [Test]
    public async Task Delete_Launches_Id_Cached()
    {
        int id = 5;
        IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(45))
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSize(100);

        Service servicecache = new Service(new FakeDBMethods(), new DbtestContext(), cache);
        var search = await servicecache.GetById(id);
        Assert.AreEqual(cache.Get(5), search.Value, null, "found the object and saved it in cache ");

        string perpage = "300";
        string page = "1";
        var result = await servicecache.Get(perpage, page);
        var count = result.Value.Count;
        LaunchInsertDTO launchElement = new LaunchInsertDTO
        {
            DateLunch = DateTime.Now,
            MissionID = 1,
            RocketID = 2,
            FirstRocketlaunch = true
        };

        var results = await servicecache.DeleteLaunch(id);
        result = await servicecache.Get(perpage, page);
        var countdelete = result.Value.Count;
        Assert.AreEqual(count - 1, countdelete, null, "deleted the object ");
        Assert.AreEqual(cache.Get(5), null, null, "deleted the object of cache");

    }
    [Test]
    public async Task Rockets_List()
    {
        
        var result = await service.GetRockets();        
        Assert.AreEqual(4, result.Value.Count, null, "solution total rockets object in fake data ");    

    }
    [Test]
    public async Task Misiion_List()
    {

        var result = await service.GetMissions();
        Assert.AreEqual(6, result.Value.Count, null, "solution total missions object in fake data ");

    }
}