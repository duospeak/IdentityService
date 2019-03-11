using Application.Queries;
using Domain.AggregatesModel;
using Infrastructure;
using IntegrationTests.Internal;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.QueryTests
{
    public class UserQueriesTests : QueryTestBase
    {
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
            var user = new ApplicationUser("username", "password");
            user.Active();
            await IdentityDbContext.ApplicationUser.AddAsync(user);
            await IdentityDbContext.SaveChangesAsync();

            var q = CreateQueries();

            // act
            var datas = await q.QueryActivedMemberUsersAsync(0, 10);

            // assert
            Assert.Single(datas);
            Assert.Equal(user.UserName, Assert.Single(datas).UserName);
        }
    }
}
