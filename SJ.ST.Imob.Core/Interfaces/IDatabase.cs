using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace SJ.ST.Imob.Core
{
    public interface IDatabase<T>
    {
        IMongoCollection<T> GetCollectionFromConnectionString(string connectionString);

        IMongoCollection<T> GetCollectionFromConnectionString(string connectionString, string collectionName);

        IMongoCollection<T> GetCollectionFromUrl(MongoUrl url);

        IMongoCollection<T> GetCollectionFromUrl(MongoUrl url, string collectionName);
    }
}
