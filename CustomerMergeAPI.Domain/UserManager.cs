using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.Domain.Models;
using CustomerMergeAPI.Domain.Repositories.Interfaces;

namespace CustomerMergeAPI.Data.Managers;

public class UserManager : IUserManager
{
    private readonly IUserRepository _repo;

    public UserManager(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<User?> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _repo.GetUserByUsernameAsync(username, cancellationToken);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public Task<List<string>> GetUserRolesAsync(int userId, CancellationToken cancellationToken)
        => _repo.GetRolesAsync(userId, cancellationToken);

    public Task CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        => _repo.CreateUserAsync(request, cancellationToken);
}
