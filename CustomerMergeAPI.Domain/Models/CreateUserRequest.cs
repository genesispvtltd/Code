using System.Collections.Generic;

namespace CustomerMergeAPI.Domain.Models;

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
