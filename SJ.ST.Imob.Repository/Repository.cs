using MongoDB.Driver;
using SJ.ST.Imob.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SJ.ST.Imob.Repository
{
    public class Repository<T> : IRepository<T>
        where T : IEntity
    {
        public Repository(IDatabase<T> database)
        {
            this.Collection = database.GetCollectionFromConnectionString("mongodb://mongodb:27017/imobilizados");
        }

        public FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;

        public ProjectionDefinitionBuilder<T> Project => Builders<T>.Projection;

        public UpdateDefinitionBuilder<T> Updater => Builders<T>.Update;

        public bool Any(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IMongoCollection<T> Collection
        {
            get; private set;
        }

        private IFindFluent<T, T> Query(Expression<Func<T, bool>> filter)
        {
            return Collection.Find(filter);
        }

        private IFindFluent<T, T> Query()
        {
            return Collection.Find(Filter.Empty);
        }

        #region CRUD

        #region Delete

        public bool Delete(string id)
        {
            return Collection.DeleteOne(i => i.Id == id).IsAcknowledged;
        }

        public bool Delete(T entity)
        {
            return Delete(entity.Id);
        }

        public bool Delete(Expression<Func<T, bool>> filter)
        {
            return Collection.DeleteMany(filter).IsAcknowledged;
        }


        #endregion Delete

        #region Find
        public IEnumerable<T> Find(Expression<Func<T, bool>> filter)
        {
            return Query(filter).ToEnumerable();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size)
        {
            return Find(filter, i => i.Id, pageIndex, size);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return Find(filter, order, pageIndex, size, true);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = Query(filter).Skip(pageIndex * size).Limit(size);
            return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable();
        }

        #endregion Find

        #region FindAll

        public IEnumerable<T> FindAll()
        {
            return Query().ToEnumerable();
        }

        public IEnumerable<T> FindAll(int pageIndex, int size)
        {
            return FindAll(i => i.Id, pageIndex, size);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return FindAll(order, pageIndex, size, true);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = Query().Skip(pageIndex * size).Limit(size);
            return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable();
        }

        #endregion Find

        #region Get

        public T Get(string id)
        {
            return Find(i => i.Id == id).FirstOrDefault();
        }

        #endregion Get

        #region Insert

        public void Insert(T entity)
        {
            Collection.InsertOne(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            Collection.InsertMany(entities);
        }

        #endregion Insert

        #region Replace

        public virtual bool Replace(T entity)
        {

            return Collection.ReplaceOne(i => i.Id == entity.Id, entity).IsAcknowledged;

        }

        public void Replace(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Replace(entity);
            }
        }

        #endregion Replace

        public bool Update(string id, params UpdateDefinition<T>[] updates)
        {
            return Update(Filter.Eq(i => i.Id, id), updates);
        }

        public bool Update(T entity, params UpdateDefinition<T>[] updates)
        {
            return Update(entity.Id, updates);
        }

        public bool Update(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates)
        {
            var update = Updater.Combine(updates).CurrentDate(i => i.Modificado);
            return Collection.UpdateMany(filter, update.CurrentDate(i => i.Modificado)).IsAcknowledged;
        }

        public bool Update(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates)
        {
            var update = Updater.Combine(updates).CurrentDate(i => i.Modificado);
            return Collection.UpdateMany(filter, update).IsAcknowledged;
        }

        public bool Update<TField>(T entity, Expression<Func<T, TField>> field, TField value)
        {
            return Update(entity, Updater.Set(field, value));
        }

        public bool Update<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value)
        {
            return Update(filter, Updater.Set(field, value));
        }

        #endregion CRUD
    }
}
