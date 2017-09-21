using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;

namespace SJ.ST.Imob.Service
{
    public class RedisConnectionFactory
    {
        private static readonly Lazy<ConnectionMultiplexer> Connection;

        //private static readonly string REDIS_CONNECTIONSTRING = "REDIS_CONNECTIONSTRING";

        static RedisConnectionFactory()
        {
            //var config = new ConfigurationBuilder()
            //            .AddEnvironmentVariables()
            //            .Build();            

            //var connectionString = config[REDIS_CONNECTIONSTRING];

            //if (connectionString == null)
            //{
            //    throw new KeyNotFoundException($"Environment variable for {REDIS_CONNECTIONSTRING} was not found.");
            //}

            var options = new ConfigurationOptions();
            options.ClientName = "myredis";
            options.EndPoints.Add("myredis", 6379);
            options.AbortOnConnectFail = false;            

            Connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
        }

        public static ConnectionMultiplexer GetConnection() => Connection.Value;
    }
}
