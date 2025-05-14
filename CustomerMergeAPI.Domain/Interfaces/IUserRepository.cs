using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomerMergeAPI.Domain.Models;


namespace CustomerMergeAPI.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<List<string>> GetRolesAsync(int userId, CancellationToken cancellationToken);
        Task CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
    }
}
