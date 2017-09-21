using System.Collections.Generic;

namespace SJ.ST.Imob.Core
{
    public interface IServiceBus<T> 
        where T : IEntity
    {
        IEnumerable<T> GetData(IRedisDataAgent redisDataAgent);

        IEnumerable<T> GetData();

        IServiceBus<T> AddRedisDataAgent(IRedisDataAgent redisDataAgent);
        IServiceBus<T> AddRepository(IRepository<T> repository);
        T GetData(string id);
        void PostData(T value);
        void PutData(string id, T value);
        void DeleteData(string id);
    }
}
