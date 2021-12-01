using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity, TKey> : IAsyncRepository<TEntity, TKey> 
        where TEntity : EntityBase<TKey>, new()
    {
        protected readonly OrderDbContext dbContext;
        protected readonly DbSet<TEntity> entities;

        public RepositoryBase(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
            entities = dbContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await entities.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await entities.Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includes = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = (disableTracking) ? entities.AsNoTracking() : entities;

            if (!string.IsNullOrWhiteSpace(includes))
                query = query.Include(includes);

            if (predicate is not null)
                query = query.Where(predicate);

            if (orderBy is not null)
                query = orderBy(query);

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = (disableTracking) ? entities.AsNoTracking() : entities;

            if (includes is not null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate is not null)
                query = query.Where(predicate);
            
            if (orderBy is not null)
                query = orderBy(query);

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await entities.FindAsync(id, cancellationToken);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await entities.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entities.Remove(entity);
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = new TEntity()
            {
                Id = id
            };

            return this.DeleteAsync(entity, cancellationToken);
        }
    }

    public class RepositoryBase<TEntity> : RepositoryBase<TEntity, int>
        where TEntity : EntityBase, new()
    {
        public RepositoryBase(OrderDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
