using Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SeedWork
{
    public interface IEntity
    {
        long Id { get; }
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        void AddDomainEvent(DomainEvent domainEvent);

        void ClearDomainEvents();
    }
}
