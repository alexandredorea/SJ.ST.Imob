using System.Collections.Generic;

namespace SJ.ST.Imob.Core
{
    public interface IServiceBus<T> 
        where T : IEntity
    {
        IEnumerable<T> GetData(IRedisDataAgent redisDataAgent);

        IEnumerable<T> GetData();

        IEnumerable<T> GetData(IDictionary<string, string> query);

        T GetData(string id);
        
        IServiceBus<T> AddRedisDataAgent(IRedisDataAgent redisDataAgent);
        
        void PostData(T value);

        void PutData(string id, T value);

        void DeleteData(string id);
        
    }
}
