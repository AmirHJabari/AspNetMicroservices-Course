using System;

namespace EventBus.Messages.Events
{
    public class BaseEvent
    {
        public BaseEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTimeOffset.UtcNow;
        }

        public BaseEvent(Guid id, DateTimeOffset creationDate)
        {
            this.Id = id;
            this.CreationDate = creationDate;
        }

        public Guid Id { get; }
        public DateTimeOffset CreationDate { get; }
    }
}
