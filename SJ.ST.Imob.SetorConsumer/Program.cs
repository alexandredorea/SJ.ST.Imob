using SimpleInjector;
using SJ.ST.Imob.Consumer;
using SJ.ST.Imob.Core;
using SJ.ST.Imob.Repository;
using System;

namespace SJ.ST.Imob.SetorConsumer
{
    static class Program
    {
        static readonly Container container;

        static Program() {
            container = new Container();

            // 2. Configure the container (register)
            container.Register<IRepository<Setor>, Repository<Setor>>();
        }

        static void Main(string[] args)
        {
            var repository = container.GetInstance<Repository<Setor>>();

            Consumer.Consumer.Get<Setor>(repository);
            Consumer.Consumer.GetList<Setor>(repository);
            Consumer.Consumer.Insert<Setor>(repository);
            Consumer.Consumer.Update<Setor>(repository);
            Consumer.Consumer.Delete<Setor>(repository);

            Console.WriteLine("Setor Hello World!");
        }
    }
}
