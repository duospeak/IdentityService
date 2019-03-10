using Application.Queries;
using Domain.AggregatesModel;
using Infrastructure;
using IntegrationTests.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.QueryTests
{
    public class UserQueriesTests : IDisposable
    {
        private readonly IReadOnlyCollection<ApplicationUser> _seedTestDatas;
        public UserQueriesTests()
        {
            _seedTestDatas = new List<ApplicationUser>();

            // add seed datas
        }
        private UserQueries CreateQueries()
        {
            return new UserQueries(new TestOptions<InfraOptions>(new InfraOptions()
            {
                ConnectionString = TestConfig.ConnectionString
            }));
        }

        [ExternalResourceFact(ExternalResource.Postgresql)]
        public async Task Add_And_QueryActivedUser_Ok()
        {
            // arrange
            var q = CreateQueries();

            // act
            var datas = await q.QueryActivedMemberUsersAsync(0, 10);

            // assert
            Assert.NotNull(datas);
        }

        public void Dispose()
        {
            // connect database and clean up
        }
    }
}
