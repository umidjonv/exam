using ServiceStack.Redis;

namespace EX.Web.Services
{
    public class CacheService
    {

        private readonly IRedisClient _client;

        public CacheService(RedisManagerPool redis)
        {
            _client = redis.GetClient();
        }

        public T Get<T>(string key) where T : class
        { 
            return _client.Get<T>(key);
        }

        public bool Set<T>(string key, T t) where T : class
        { 
            return _client.Set(key, t);
        }

        public void Remove<T>(string key) where T : class
        {
            _client.DeleteById<T>(key);
        }

    }
}
