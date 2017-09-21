using SimpleInjector;
using SJ.ST.Imob.Core;
using SJ.ST.Imob.Repository;
using System;
using Xunit;

namespace JS.ST.Imob.RepositoryTest
{
    public class TesteDeInjecao
    {
        private Container container;


        [Fact]
        public void TestRegister()
        {
            container = new Container();

            var types = container.GetTypesToRegister(typeof(IRepository<>),
        new[] { typeof(Repository<>).Assembly });

            container.Register(typeof(IDatabase<>), typeof(Database<>));
            //container.Register<IEntity, Entity>();

            foreach (Type type in types)
                container.Register(type);

            
                        
            var repository = container.GetInstance<SetorRepository>();

            Assert.IsType<SetorRepository>(repository);
        }
    }
}
