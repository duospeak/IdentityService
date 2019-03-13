using Application.Models;
using Domain.Enumerations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries
{
    public interface IUserQueries
    {
        Task<IEnumerable<UserListDto>> QueryBlockedUsersAsync(int offset, int count, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserListDto>> QueryActivedUsersAsync(int offset, int count, CancellationToken cancellationToken = default);

        Task<UserStatus?> QueryStatusByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        Task<UserDto> QueryMemberUserByIdAsync(long userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserListDto>> QueryUsersAsync(string username, int offset, int count, CancellationToken cancellationToken = default);
    }
}
