using System;
using StackExchange.Redis;

namespace RedisCache
{
    class Program
    {
        private static string ConnectionString = "LeeTestCache.redis.cache.windows.net:6380,password=QAOsdhy5TiUVF9Je2GRXPCZY0b+eHKgCAud9ocV4th8=,ssl=True,abortConnect=False";
        private static Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() => {
            return ConnectionMultiplexer.Connect(ConnectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get 
            {
                return LazyConnection.Value;
            }
        }

        static void Main(string[] args)
        {
            IDatabase cache = LazyConnection.Value.GetDatabase();
            cache.StringSet("cacheItem1", "Some cached data 1");
            cache.StringSet("cacheItem2", "Some cached data 2");
            cache.KeyExpire("cacheItem1", DateTime.Now.AddMinutes(1));

            string cachedValue = cache.StringGet("cacheItem1").ToString();

            Console.WriteLine($"Retrieved cached value {cachedValue}");
            Console.ReadLine();
        }        
    }
}
