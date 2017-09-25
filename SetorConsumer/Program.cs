using SimpleInjector;
using SJ.ST.Imob.Consumer;
using SJ.ST.Imob.Core;
using SJ.ST.Imob.Repository;
using System;
using System.Threading.Tasks;

namespace SetorConsumer
{
    static class Program
    {
        static readonly Container container;
        static Program()
        {
            container = new Container();

            // 2. Configure the container (register)
            container.Register<IRepository<Setor>, Repository<Setor>>();
            container.Register<IDatabase<Setor>, Database<Setor>>();

            container.Verify();
        }

        static void Main(string[] args)
        {
            var repository = container.GetInstance<Repository<Setor>>();
            Task.Factory.StartNew(() =>
            {
                Consumer.Get<Setor>(repository);
                Consumer.GetList<Setor>(repository);
                Consumer.Insert<Setor>(repository);
                Consumer.Update<Setor>(repository);
                Consumer.Delete<Setor>(repository);
            });

            Console.WriteLine("Setor Hello World!");
        }
    }
}
