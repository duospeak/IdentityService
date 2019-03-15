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

        public UserRepository(IdentityServiceContext context)
            => _context = context.NotNull(nameof(context));

        public IUnitOfWork UnitOfWork => _context;

        public async Task<ApplicationUser> AddAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            user.NotNull(nameof(user));

            var entry = await _context.ApplicationUser.AddAsync(user, cancellationToken);

            return entry.Entity;
        }
        public async Task<ApplicationUser> GetAsync(string userName, CancellationToken cancellationToken = default)
        {
            userName.NotNull(nameof(userName));

            var user = await _context.ApplicationUser
                  .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

            return user;
        }

        public void Update(ApplicationUser user) => _context.ApplicationUser.Update(user);
    }
}
