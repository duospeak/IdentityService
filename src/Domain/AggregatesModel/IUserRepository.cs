using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.AggregatesModel
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetAsync(string userName, CancellationToken cancellationToken = default);

        Task<ApplicationUser> AddAsync(ApplicationUser user, CancellationToken cancellationToken = default);

        void Update(ApplicationUser user);
    }
}
