using System;
using System.Collections.Generic;
using System.Text;
using Domain.Events;

namespace Domain.SeedWork
{
    public abstract class Entity : IEntity
    {
        protected Entity() => _domainEvents = new List<DomainEvent>();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        private readonly List<DomainEvent> _domainEvents;
        public long Id { get; protected set; }

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
