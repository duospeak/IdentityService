using Domain.AggregatesModel;
using Domain.SeedWork;
using Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityServiceContext _context;
        public IUnitOfWork UnitOfWork { get; }

        public async Task<ApplicationUser> AddAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            user.NotNull(nameof(user));

            var entry = await _context.ApplicationUser.AddAsync(user, cancellationToken);

            return entry.Entity;
        }
        public async Task<ApplicationUser> GetAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _context.ApplicationUser
                  .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

            return user;
        }
    }
}
