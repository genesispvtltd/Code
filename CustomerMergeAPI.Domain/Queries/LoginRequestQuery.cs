using MediatR;

namespace CustomerMergeAPI.Domain.Queries;

public class LoginRequestQuery : IRequest<string?>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}