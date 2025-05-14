using CustomerMergeAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerMergeAPI.Domain.Interfaces;

public interface IUserManager
{
    Task<User?> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken);
    Task<List<string>> GetUserRolesAsync(int userId, CancellationToken cancellationToken);
    Task CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
}
