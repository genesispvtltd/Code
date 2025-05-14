using MediatR;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.Domain.Queries;

namespace CustomerMergeAPI.WebApi.Handlers;

public class LoginRequestQueryHandler : IRequestHandler<LoginRequestQuery, string?>
{
    private readonly IUserManager _userManager;
    private readonly IConfiguration _config;

    public LoginRequestQueryHandler(IUserManager userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<string?> Handle(LoginRequestQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.AuthenticateUserAsync(request.Username, request.Password, cancellationToken);
        if (user == null)
            return null;

        var roles = await _userManager.GetUserRolesAsync(user.UserId, cancellationToken);

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
