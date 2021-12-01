using System;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase<TKey> : IEntity
    {
        public virtual TKey Id { get; set; }

        public virtual DateTimeOffset CreatedDate { get; set; }
        public virtual DateTimeOffset LastModifiedDate { get; set; }
    }

    public abstract class EntityBase : EntityBase<int>
    {
    }
}
