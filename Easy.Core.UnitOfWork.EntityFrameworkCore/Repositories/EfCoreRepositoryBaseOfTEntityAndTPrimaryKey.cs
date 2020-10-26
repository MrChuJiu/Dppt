using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Easy.Core.Domain.Entities;
using Easy.Core.Flow.AspectCore.Extensions;
using Easy.Core.Flow.Domain.Repositories;
using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Uow.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Easy.Core.Flow.UnitOfWork.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TKey">Primary key of the entity</typeparam>
    public class EfCoreRepositoryBase<TEntity, TKey> : AppRepositoryBase<TEntity, TKey>,
        IRepositoryWithDbContext
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public virtual DbContext Context => UnitOfWorkDbContextProvider.GetDbContext();

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        /// <summary>
        /// Gets DbQuery for given entity.
        /// </summary>
        public virtual DbSet<TEntity> DbQueryTable => Context.Set<TEntity>();

        private static readonly ConcurrentDictionary<Type, bool> EntityIsDbQuery =
            new ConcurrentDictionary<Type, bool>();

        public virtual DbTransaction Transaction
        {
            get
            {
                return (DbTransaction)TransactionProvider?.GetActiveTransaction();
            }
        }

        public virtual DbConnection Connection
        {
            get
            {
                var connection = Context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }
        }

        public virtual IActiveTransactionProvider TransactionProvider { get; private set; }

        public virtual IUnitOfWorkDbContextProvider UnitOfWorkDbContextProvider { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionProvider"></param>
        /// <param name="dbContextProvider"></param>
        public EfCoreRepositoryBase(
            IActiveTransactionProvider transactionProvider,
            IUnitOfWorkDbContextProvider unitOfWorkDbContextProvider
            )
        {
            TransactionProvider = transactionProvider;
            UnitOfWorkDbContextProvider = unitOfWorkDbContextProvider;
        }

        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TKey id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public override TKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = Update(entity);
            return Task.FromResult(entity);
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void Delete(TKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            //Could not found the entity, do nothing.
        }

        public override async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public override async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        public virtual DbContext GetDbContext()
        {
            return Context;
        }

        public virtual Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Collection(collectionExpression).LoadAsync(cancellationToken);
        }

        public virtual void EnsureCollectionLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            Context.Entry(entity).Collection(collectionExpression).Load();
        }

        public virtual Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }

        public virtual void EnsurePropertyLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            Context.Entry(entity).Reference(propertyExpression).Load();
        }


        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return Table.AsQueryable();
        }


        private TEntity GetFromChangeTrackerOrNull(TKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        private static bool MayHaveTemporaryKey(TEntity entity)
        {
            if (typeof(TKey) == typeof(byte))
            {
                return true;
            }

            if (typeof(TKey) == typeof(int))
            {
                return Convert.ToInt32(entity.Id) <= 0;
            }

            if (typeof(TKey) == typeof(long))
            {
                return Convert.ToInt64(entity.Id) <= 0;
            }

            return false;
        }
    }
}
