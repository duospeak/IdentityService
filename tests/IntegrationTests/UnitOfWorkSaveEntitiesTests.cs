using Domain.AggregatesModel;
using Infrastructure.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class UnitOfWorkSaveEntitiesTests : TestBase
    {
        private IdentityServiceContext _context;

        [DockerFact]
        public async Task AddAndGetReturnsObject()
        {
            await StartNpgsqlServer();

            var username = Guid.NewGuid().ToString();
            // adding
        }
    }
}
