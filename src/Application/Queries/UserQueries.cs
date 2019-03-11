using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Dapper;
using Domain.Enumerations;
using Infrastructure;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Application.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly InfraOptions _options;

        public UserQueries(IOptions<InfraOptions> options)
        {
            _options = options.NotNull(nameof(options)).Value;
        }

        public async Task<IEnumerable<UserListDto>> QueryActivedMemberUsersAsync(int offset, int count, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_options.ConnectionString))
            {
                return await connection.QueryAsync<UserListDto>("SELECT \"UserName\",\"Status\" FROM \"ApplicationUser\" limit @count offset @offset", new
                {
                    offset,
                    count
                });
            }
        }
        public Task<IEnumerable<UserListDto>> QueryBlockedMemberUsersAsync(int offset, int count, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<IEnumerable<UserListDto>> QueryManagersAsync(int offset, int count, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<UserDto> QueryMemberUserByIdAsync(long userId, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<IEnumerable<UserListDto>> QueryMemberUsersAsync(int offset, int count, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<UserStatus> QueryStatusByUserIdAsync(long userId, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
