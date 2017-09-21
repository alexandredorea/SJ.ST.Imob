using SJ.ST.Imob.Core;
using StackExchange.Redis;

namespace SJ.ST.Imob.Service
{
    public class RedisDataAgent : IRedisDataAgent
    {
        private static IDatabase _database;
        public RedisDataAgent()
        {
            var connection = RedisConnectionFactory.GetConnection();

            _database = connection.GetDatabase();
        }

        public string GetStringValue(string key)
        {
            return _database.StringGet(key);
        }

        public void SetStringValue(string key, string value)
        {
            _database.StringSet(key, value);
        }

        public void DeleteStringValue(string key)
        {
            _database.KeyDelete(key);
        }
    }
}
