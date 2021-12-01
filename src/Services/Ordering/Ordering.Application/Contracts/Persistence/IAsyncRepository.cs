using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ordering.Domain.Common;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IAsyncRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
	{
		Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
		Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
										Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
										string includeString = null,
										bool disableTracking = true, CancellationToken cancellationToken = default);

		Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
									   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
									   List<Expression<Func<TEntity, object>>> includes = null,
									   bool disableTracking = true, CancellationToken cancellationToken = default);
		Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

		Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
	}

	public interface IAsyncRepository<TEntity> : IAsyncRepository<TEntity, int>
		where TEntity : EntityBase
    {
    }
}
