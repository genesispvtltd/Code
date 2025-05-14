using MediatR;
using System.Collections.Generic;

namespace CustomerMergeAPI.Domain.Commands;

public class CreateUserCommand : IRequest<Unit>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
