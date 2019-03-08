using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SeedWork
{
    public interface IRepository<TModel>
        where TModel : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
