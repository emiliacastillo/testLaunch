﻿/*using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    }
}
