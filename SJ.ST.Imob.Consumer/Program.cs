using EasyNetQ;
using SimpleInjector;
using SJ.ST.Imob.Core;
using SJ.ST.Imob.Repository;
using System;
using System.Threading.Tasks;

namespace SJ.ST.Imob.Consumer
{
    class Program
    {
        static readonly Container container;

        static Program()
        {
            container = new Container();

            var types = container.GetTypesToRegister(typeof(IRepository<>),
        new[] { typeof(Repository<>).Assembly });

            container.Register(typeof(IDatabase<>), typeof(Database<>));
            //container.Register<IEntity, Entity>();

            foreach (Type type in types)
                container.Register(type);
        }

        static void Main(string[] args)
        {
            #region Post
            Consume<Setor>("Post");
            Consume<Natureza>("Post");
            Consume<Classe>("Post");
            Consume<Motivo>("Post");
            Consume<Imobilizado>("Post");
            Consume<Movimento>("Post");
            Consume<Status>("Post");
            Consume<Subclasse>("Post");
            #endregion

            #region Delete
            Consume<Setor>("Delete");
            Consume<Natureza>("Delete");
            Consume<Classe>("Delete");
            Consume<Motivo>("Delete");
            Consume<Imobilizado>("Delete");
            Consume<Movimento>("Delete");
            Consume<Status>("Delete");
            Consume<Subclasse>("Delete");
            #endregion

            #region Put
            Consume<Setor>("Put");
            Consume<Natureza>("Put");
            Consume<Classe>("Put");
            Consume<Motivo>("Put");
            Consume<Imobilizado>("Put");
            Consume<Movimento>("Put");
            Consume<Status>("Put");
            Consume<Subclasse>("Put");
            #endregion

        }

        static void Consume<T>(string queueString) where T : class, IEntity
        {
            var advancedBus = RabbitHutch.CreateBus("host=myrabbitmq").Advanced;
            var queue = advancedBus.QueueDeclare(queueString);

            advancedBus.Consume<T>(queue, (mensage, info) => Task.Factory.StartNew(() =>
            {
                switch (queueString)
                {
                    case "Post":
                        Post<T>(mensage.Body);
                        break;
                    case "Put":
                        Put<T>(mensage.Body);
                        break;
                    case "Delete":
                        Delete<T>(mensage.Properties.CorrelationId);
                        break;
                }

                Console.WriteLine("Got message: '{0}'", mensage);
            }));
        }

        static void Post<T>(T body) where T : class, IEntity
        {
            var repository = container.GetInstance<Repository<T>>();
           
            repository.Insert(body);
        }

        static void Put<T>(T body) where T : class, IEntity
        {
            var repository = container.GetInstance<Repository<T>>();
            
            repository.Replace(body);
        }

        static void Delete<T>(string id) where T : class, IEntity
        {
            var repository = container.GetInstance<Repository<T>>();
            repository.Delete(id);
        }
    }
}
