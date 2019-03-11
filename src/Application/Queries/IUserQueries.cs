using Application.Models;
using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries
{
    public interface IUserQueries
    {
        Task<IEnumerable<UserListDto>> QueryMemberUsersAsync(int offset,int count,CancellationToken cancellationToken=default);

        Task<IEnumerable<UserListDto>> QueryBlockedMemberUsersAsync(int offset, int count, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserListDto>> QueryActivedMemberUsersAsync(int offset, int count, CancellationToken cancellationToken = default);

        Task<UserStatus?> QueryStatusByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        Task<UserDto> QueryMemberUserByIdAsync(long userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserListDto>> QueryManagersAsync(int offset, int count, CancellationToken cancellationToken = default);
    }
}
