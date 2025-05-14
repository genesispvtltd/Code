using CustomerMergeAPI.Domain.Models;
using CustomerMergeAPI.Domain.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerMergeAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;

        public UserRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync(cancellationToken);
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<List<string>> GetRolesAsync(int userId, CancellationToken cancellationToken)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync(cancellationToken);
            var sql = @"
                SELECT r.RoleName
                FROM Roles r
                JOIN UserRoles ur ON ur.RoleId = r.RoleId
                WHERE ur.UserId = @UserId";
            var roles = await conn.QueryAsync<string>(sql, new { UserId = userId });
            return roles.ToList();
        }

        public async Task CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync(cancellationToken);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var rolesCsv = string.Join(",", request.Roles);

            await conn.ExecuteAsync(
                          "sp_CreateUserWithRoles",
        new { Username = request.Username, PasswordHash = hashedPassword, Roles = rolesCsv },
        commandType: CommandType.StoredProcedure
                       );
        }
    }
}
