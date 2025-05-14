using MediatR;
using CustomerMergeAPI.Domain.Commands;
using CustomerMergeAPI.Domain.Interfaces;

namespace CustomerMergeAPI.WebApi.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly IUserManager _userManager;

    public CreateUserHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _userManager.CreateUserAsync(new Domain.Models.CreateUserRequest
        {
            Username = request.Username,
            Password = request.Password,
            Roles = request.Roles
        }, cancellationToken);

        return Unit.Value;
    }
}
