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
        
        public IServiceBus<T> AddRedisDataAgent(IRedisDataAgent redisDataAgent)
        {
            this.redisDataAgent = redisDataAgent;
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
                return GetDataFromDB();
        }

        public IEnumerable<T> GetData(IDictionary<string, string> query)
        {
            var lista = redisDataAgent.GetStringValue(query.ToString() + typeof(T).Name);

            if (!string.IsNullOrEmpty(lista))
                return JsonConvert.DeserializeObject<IList<T>>(lista);
            else
                return GetDataFromDB(query);
        }        

        public T GetData(string id)
        {
            var objeto = redisDataAgent.GetStringValue(id);

            if (!string.IsNullOrEmpty(objeto))
                return JsonConvert.DeserializeObject<T>(objeto);
            else
                return GetDataFromDB(id);
        }

        private T GetDataFromDB(string id)
        {
            T objeto = null;

            using (IBus bus = RabbitHutch.CreateBus(RABBIT_HOST))
            {
                objeto = bus.Request<string, T>(id, x =>
                {
                    x.WithQueueName("Get-" + typeof(T).Name);
                });
            }

            redisDataAgent.DeleteStringValue(id);
            redisDataAgent.SetStringValue(id, JsonConvert.SerializeObject(objeto));

            return objeto;
        }

        private IList<T> GetDataFromDB()
        {
            IList<T> lista = null;

            using (IBus bus = RabbitHutch.CreateBus(RABBIT_HOST))
            {
                lista = bus.Request<string, IList<T>>("Lista", x =>
                {
                    x.WithQueueName("GetList-" + typeof(T).Name);
                });
            }

            redisDataAgent.DeleteStringValue("Lista" + typeof(T).Name);
            redisDataAgent.SetStringValue("Lista" + typeof(T).Name, JsonConvert.SerializeObject(lista));

            return lista;
        }

        private IList<T> GetDataFromDB(IDictionary<string, string> query)
        {
            IList<T> objeto = null;

            using (IBus bus = RabbitHutch.CreateBus(RABBIT_HOST))
            {
                objeto = bus.Request<IDictionary<string, string>, IList<T>>(query, x =>
                {
                    x.WithQueueName("GetList-" + typeof(T).Name);
                });
            }

            redisDataAgent.DeleteStringValue(query.ToString() + typeof(T).Name);
            redisDataAgent.SetStringValue(query.ToString() + typeof(T).Name, JsonConvert.SerializeObject(objeto));

            return objeto;
        }

        public void PostData(T value)
        {
            var Advancedbus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = Advancedbus.QueueDeclare("Post-" + typeof(T).Name);

            if (Advancedbus.IsConnected)
                Advancedbus.Publish(Exchange.GetDefault(), queue.Name, true, new Message<T>(value));
            else
                throw new Exception("Erro na conexão");

            redisDataAgent.DeleteStringValue("Lista" + typeof(T).Name);
        }

        public void PutData(string id, T body)
        {
            var Advancedbus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = Advancedbus.QueueDeclare("Post-" + typeof(T).Name);

            if (Advancedbus.IsConnected)
                Advancedbus.Publish(Exchange.GetDefault(), queue.Name, true, new Message<T>(body));
            else
                throw new Exception("Erro na conexão");

            redisDataAgent.DeleteStringValue("Lista" + typeof(T).Name);
            redisDataAgent.DeleteStringValue(id);

        }

        public void DeleteData(string id)
        {
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
