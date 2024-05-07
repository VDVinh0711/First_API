using StackExchange.Redis;
using System.Text.Json;


namespace API_EF.Service
{
    public class CachesSystem : ICaches

    {
       IDatabase _db;

        


        public CachesSystem()
        {

            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _db = redis.GetDatabase();

        }


        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
          
        }

        public object RemoveData<T>(string key)
        {
            var _exitsdata = _db.KeyExists(key);
            if(_exitsdata)
            {
               return _db.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expriteTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _db.StringSet(key, JsonSerializer.Serialize(value), expriteTime);
           
        }
    }
}
