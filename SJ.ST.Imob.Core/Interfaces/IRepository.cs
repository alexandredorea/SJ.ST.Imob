using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SJ.ST.Imob.Core
{
    
    public interface IRepository<T> where T : IEntity
    {
        IMongoCollection<T> Collection { get; }

        FilterDefinitionBuilder<T> Filter { get; }

        ProjectionDefinitionBuilder<T> Project { get; }

        UpdateDefinitionBuilder<T> Updater { get; }

        #region CRUD

        #region Delete

        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id</param>
        bool Delete(string id);


        /// <summary>
        /// delete entity
        /// </summary>
        /// <param name="entity">entity</param>
        bool Delete(T entity);

        /// <summary>
        /// delete items with filter
        /// </summary>
        /// <param name="filter">expression filter</param>
        bool Delete(Expression<Func<T, bool>> filter);

        
        #endregion Delete

        #region Find

        /// <summary>
        /// find entities
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter);

        /// <summary>
        /// find entities with paging
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size);

        /// <summary>
        /// find entities with paging and ordering
        /// default ordering is descending
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size);

        /// <summary>
        /// find entities with paging and ordering in direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);

        #endregion Find

        #region FindAll

        IEnumerable<T> FindAll();

        IEnumerable<T> FindAll(int pageIndex, int size);

        IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size);

        IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);

        #endregion FindAll
        
        #region Get

        T Get(string id);

        #endregion Get

        #region Insert

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        #endregion Insert

        #region Update

        bool Update(string id, params UpdateDefinition<T>[] updates);

        bool Update(T entity, params UpdateDefinition<T>[] updates);

        bool Update(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates);

        bool Update(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates);

        bool Update<TField>(T entity, Expression<Func<T, TField>> field, TField value);

        bool Update<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value);

        #endregion Update

        #region Replace

        bool Replace(T entity);

        void Replace(IEnumerable<T> entities);

        #endregion Replace

        #endregion CRUD



    }
}