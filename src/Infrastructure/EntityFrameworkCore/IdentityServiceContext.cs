using Domain.AggregatesModel;
using Domain.SeedWork;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.EntityFrameworkCore
{
    public class IdentityServiceContext : DbContext, IUnitOfWork
    {
        private readonly ICapPublisher _publisher;
        public DbSet<IdentityServiceOptions> ServiceOptions { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityServiceContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker.Entries<IEntity>();

            var domainEvents = entries
                .SelectMany(x => x.Entity.DomainEvents)
                .Where(x => x != null)
                .ToArray();

            using (var transaction = Database.BeginTransaction(_publisher))
            {
                var tasks = domainEvents
                     .Select(e => _publisher.PublishAsync(e.GetType().Name, e, null, cancellationToken))
                     .ToArray();

                await Task.WhenAll(tasks);

                var result = await SaveChangesAsync(cancellationToken);

                //transaction.Commit();

                return result;
            }
        }
    }
}
