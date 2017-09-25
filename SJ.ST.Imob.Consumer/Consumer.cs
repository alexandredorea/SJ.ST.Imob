using EasyNetQ;
using SJ.ST.Imob.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJ.ST.Imob.Consumer
{
    public static class Consumer
    {
        private static readonly string RABBIT_HOST = "host=rabbitmq";

        public static void Update<T>(IRepository<T> repository) where T : class, IEntity
        {
            var advancedBus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = advancedBus.QueueDeclare("Put-" + typeof(T).Name);

            advancedBus.Consume<T>(queue, (msg, properties) =>
            {
                repository.Replace(msg.Body);
            });
        }

        public static void Insert<T>(IRepository<T> repository) where T : class, IEntity
        {
            var advancedBus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = advancedBus.QueueDeclare("Post-" + typeof(T).Name);

            advancedBus.Consume<T>(queue, (msg, properties) =>
            {
                repository.Insert(msg.Body);
            });
        }

        public static void Delete<T>(IRepository<T> repository) where T : class, IEntity
        {
            var advancedBus = RabbitHutch.CreateBus(RABBIT_HOST).Advanced;
            var queue = advancedBus.QueueDeclare("Delete-" + typeof(T).Name);

            advancedBus.Consume<T>(queue, (body, properties) =>
            {
                repository.Delete(body.Body);
            });

        }

        public static void Get<T>(IRepository<T> repository) where T : class, IEntity
        {
            using (IBus bus = RabbitHutch.CreateBus(RABBIT_HOST))
            {
                bus.RespondAsync<string, T>(x =>
                Task.Factory.StartNew(() =>
                {
                    return repository.Get(x);
                }), x =>
                {
                    x.WithQueueName("Get-" + typeof(T).Name);
                });
            }
        }

        public static void GetList<T>(IRepository<T> repository) where T : class, IEntity
        {
            using (IBus bus = RabbitHutch.CreateBus(RABBIT_HOST))
            {
                bus.RespondAsync<string, List<T>>(x =>
                Task.Factory.StartNew(() =>
                {
                    return repository.FindAll().ToList<T>();
                }), x =>
                {
                    x.WithQueueName("GetList-" + typeof(T).Name);
                });
            }
        }
    }
}
