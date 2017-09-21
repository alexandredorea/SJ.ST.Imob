using Easy = EasyNetQ;
using MQ = RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    static class Program
    {
        public const string RoutingKey = "test.new.*";
        public const string ExchangeName = "test.exchange";

        public static readonly Func<Guid, string> QueueNameFunc = (id) => string.Format("test.listening.{0}", id);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int tasksCount = 500;
            int loopCount = 500;
            int expires = 60000;

            //MQ.ConnectionFactory connectionFactory = new MQ.ConnectionFactory();
            //connectionFactory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            //MQ.IConnection connection = connectionFactory.CreateConnection();

            var easyNetQBus = Easy.RabbitHutch.CreateBus("host=myrabbitmq");

            //PerformanceTasksMq(connection, tasksCount, expires);
            PerformanceTasksEasyNetQ(easyNetQBus, tasksCount, expires);

            //PerformanceLoopMq(connection, loopCount, expires);
            PerformanceLoopEasyNetQ(easyNetQBus, loopCount, expires);

            return;
        }

        private static void PerformanceTasksMq(MQ.IConnection connection, int count, int expires)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = Enumerable.Range(1, count).Select(i =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var channel = connection.CreateModel();
                    SimpleMqTest(channel, expires);
                });
            }).ToArray();

            Task.WaitAll(tasks);

            stopwatch.Stop();

            Console.WriteLine("Time of MQ: {0:hh\\:mm\\:ss\\:fff}", stopwatch.Elapsed);
        }

        private static void PerformanceLoopMq(MQ.IConnection connection, int count, int expires)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < count; i++)
            {
                var channel = connection.CreateModel();
                SimpleMqTest(channel, expires);
            }
            stopwatch.Stop();

            Console.WriteLine("Time of MQ Loop: {0:hh\\:mm\\:ss\\:fff}", stopwatch.Elapsed);
        }

        private static void PerformanceTasksEasyNetQ(Easy.IBus bus, int count, int expires)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Easy.Topology.IExchange exchange = bus.Advanced.ExchangeDeclare(ExchangeName, Easy.Topology.ExchangeType.Topic, true);
            Easy.IAdvancedBus advancedBus = bus.Advanced;

            var tasks = Enumerable.Range(1, count).Select((i, j) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    SimpleEasyNetQTest(advancedBus, exchange, expires, j);
                });
            }).ToArray();

            Task.WaitAll(tasks);

            stopwatch.Stop();
            Console.WriteLine("Time of EasyNetQ: {0:hh\\:mm\\:ss\\:fff}", stopwatch.Elapsed);
        }

        private static void PerformanceLoopEasyNetQ(Easy.IBus bus, int count, int expires)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Easy.Topology.IExchange exchange = bus.Advanced.ExchangeDeclare(ExchangeName, Easy.Topology.ExchangeType.Topic, true);
            Easy.IAdvancedBus advancedBus = bus.Advanced;

            for (int i = 0; i < count; i++)
            {
                SimpleEasyNetQTest(advancedBus, exchange, expires, i);
            }

            stopwatch.Stop();
            Console.WriteLine("Time of EasyNetQ Loop: {0:hh\\:mm\\:ss\\:fff}", stopwatch.Elapsed);
        }

        private static void SimpleMqTest(MQ.IModel channel, int expires)
        {
            var arguments = new Dictionary<string, object> { { "x-expires", expires } };
            var queueName = QueueNameFunc(Guid.NewGuid());
            channel.QueueDeclare(queueName, false, true, true, arguments);
            channel.QueueBind(queueName, ExchangeName, RoutingKey, arguments);
        }

        private static void SimpleEasyNetQTest(Easy.IAdvancedBus advancedBus, Easy.Topology.IExchange exchange, int expires, int iter)
        {
            Console.WriteLine("Start iter of EasyNetQ: {0}", iter + 1);
            Easy.Topology.IQueue queue = advancedBus.QueueDeclare(QueueNameFunc(Guid.NewGuid()), false, true, true, expires: expires);
            advancedBus.Bind(exchange, queue, RoutingKey);
            Console.WriteLine("End iter of EasyNetQ: {0}", iter + 1);
        }
    }
}
