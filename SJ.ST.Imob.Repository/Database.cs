using MongoDB.Driver;
using System;
using System.Reflection;
using SJ.ST.Imob.Core;

namespace SJ.ST.Imob.Repository
{
    public class Database<T> : IDatabase<T>
        where T : IEntity
    {
       
        public IMongoCollection<T> GetCollectionFromConnectionString(string connectionString)
        {
            return GetCollectionFromConnectionString(connectionString, GetCollectionName());
        }

        public IMongoCollection<T> GetCollectionFromConnectionString(string connectionString, string collectionName)
        {
            return GetCollectionFromUrl(new MongoUrl(connectionString), collectionName);
        }

        public IMongoCollection<T> GetCollectionFromUrl(MongoUrl url)
        {
            return GetCollectionFromUrl(url, GetCollectionName());
        }

        public IMongoCollection<T> GetCollectionFromUrl(MongoUrl url, string collectionName)
        {
            return GetDatabaseFromUrl(url).GetCollection<T>(collectionName);
        }


        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            return client.GetDatabase(url.DatabaseName); // WriteConcern defaulted to Acknowledged
        }

        private string GetCollectionName()
        {
            string collectionName;
            collectionName = typeof(T).GetTypeInfo().BaseType.Equals(typeof(object)) ?
                                      GetCollectionNameFromInterface() :
                                      GetCollectionNameFromType();

            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(T).Name;
            }
            return collectionName.ToLowerInvariant();
        }

        private string GetCollectionNameFromInterface()
        {
            var att = CustomAttributeExtensions.GetCustomAttribute<CollectionNameAttribute>(typeof(T).GetTypeInfo().Assembly);

            return att?.Name ?? typeof(T).Name;
        }

        private string GetCollectionNameFromType()
        {
            Type entitytype = typeof(T);
            string collectionname;

            var att = CustomAttributeExtensions.GetCustomAttribute<CollectionNameAttribute>(typeof(T).GetTypeInfo().Assembly);
            if (att != null)
            {
                collectionname = att.Name;
            }
            else
            {
                collectionname = entitytype.Name;
            }

            return collectionname;
        }


        #region Connection Name

        private string GetConnectionName()
        {
            string connectionName;
            connectionName = typeof(T).GetTypeInfo().BaseType.Equals(typeof(object)) ?
                                      GetConnectionNameFromInterface() :
                                      GetConnectionNameFromType();

            if (string.IsNullOrEmpty(connectionName))
            {
                connectionName = typeof(T).Name;
            }
            return connectionName.ToLowerInvariant();
        }

        private string GetConnectionNameFromInterface()
        {
            var att = CustomAttributeExtensions.GetCustomAttribute<ConnectionNameAttribute>(typeof(T).GetTypeInfo().Assembly);
            return att?.Name ?? typeof(T).Name;
        }

        private string GetConnectionNameFromType()
        {
            Type entitytype = typeof(T);
            string collectionname;

            var att = CustomAttributeExtensions.GetCustomAttribute<ConnectionNameAttribute>(typeof(T).GetTypeInfo().Assembly);
            if (att != null)
            {
                collectionname = att.Name;
            }
            else
            {
                if (typeof(Entity).GetTypeInfo().IsAssignableFrom(entitytype))
                {
                    while (!entitytype.GetTypeInfo().BaseType.Equals(typeof(Entity)))
                    {
                        entitytype = entitytype.GetTypeInfo().BaseType;
                    }
                }
                collectionname = entitytype.Name;
            }

            return collectionname;
        }

        #endregion Connection Name

    }

}