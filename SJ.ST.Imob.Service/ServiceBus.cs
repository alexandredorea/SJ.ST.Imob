using System.Collections.Generic;
using System.Linq;
using SJ.ST.Imob.Core;
using Newtonsoft.Json;
using EasyNetQ;
using EasyNetQ.Topology;
using System.Threading.Tasks;
using System;

namespace SJ.ST.Imob.Service
{
    public class ServiceBus<T> : IServiceBus<T>
         where T : class, IEntity
    {

        private readonly string RABBIT_HOST = "host=rabbitmq";

        private IRedisDataAgent redisDataAgent;
        private IRepository<T> repository;
        

        public IServiceBus<T> AddRedisDataAgent(IRedisDataAgent redisDataAgent)
        {
            this.redisDataAgent = redisDataAgent;
            return this;
        }

        public IServiceBus<T> AddRepository(IRepository<T> repository)
        {
            this.repository = repository;
            return this;
        }

        public IEnumerable<T> GetData(IRedisDataAgent redisDataAgent)
        {
            this.redisDataAgent = redisDataAgent;
            return GetData();
        }

        public IEnumerable<T> GetData()
        {
            var lista = redisDataAgent.GetStringValue("Lista" + typeof(T).Name);

            if (!string.IsNullOrEmpty(lista))
                return JsonConvert.DeserializeObject<IList<T>>(lista);
            else
                return GetDataFromRepository();
        }

        public T GetData(string id)
        {
            var objeto = redisDataAgent.GetStringValue(id);

            if (!string.IsNullOrEmpty(objeto))
                return JsonConvert.DeserializeObject<T>(objeto);
            else
                return GetDataFromRepository(id);
        }

        private T GetDataFromRepository(string id)
        {
            T objeto = repository.Get(id);

            redisDataAgent.DeleteStringValue(id);
            redisDataAgent.SetStringValue(id, JsonConvert.SerializeObject(objeto));

            return objeto;
        }

        public IEnumerable<T> GetDataFromRepository()
        {
            IEnumerable<T> lista = repository.FindAll();

            redisDataAgent.DeleteStringValue("Lista" + typeof(T).Name);
            redisDataAgent.SetStringValue("Lista" + typeof(T).Name, JsonConvert.SerializeObject(lista.ToArray()));

            return lista;
        }

        public void PostData(T value)
        {
            var Advancedbus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = Advancedbus.QueueDeclare("Post");

            if (Advancedbus.IsConnected)
                Advancedbus.Publish(Exchange.GetDefault(), queue.Name, true, new Message<T>(value));
            else
                throw new Exception("Erro na conexão");

            redisDataAgent.DeleteStringValue("Lista" + typeof(T).Name);
        }

        public void PutData(string id, T body)
        {
            var Advancedbus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = Advancedbus.QueueDeclare("Put");

            IMessage<T> message = new Message<T>(body, new MessageProperties() { CorrelationId = id });
            
            if (Advancedbus.IsConnected)
                Advancedbus.Publish(Exchange.GetDefault(), queue.Name, true, message);
            else
                throw new Exception("Erro na conexão");

            redisDataAgent.DeleteStringValue("Lista" + typeof(T).Name);
            redisDataAgent.DeleteStringValue(id);
        }

        public void DeleteData(string id) {

            IMessage<string> message = new Message<string>(id);

            var Advancedbus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = Advancedbus.QueueDeclare("Delete");

            if (Advancedbus.IsConnected)
                Advancedbus.Publish(Exchange.GetDefault(), queue.Name, true, message);
            else
                throw new Exception("Erro na conexão");

            redisDataAgent.DeleteStringValue(id);
            redisDataAgent.DeleteStringValue("Lista" + typeof(T).Name);
        }
       
    }
}
