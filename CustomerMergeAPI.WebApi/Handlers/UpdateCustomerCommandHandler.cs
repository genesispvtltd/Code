using CustomerMergeAPI.Domain.Interfaces;
using MediatR;

namespace CustomerMergeAPI.WebApi.Handlers;
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly ICustomerManager _manager;

    public UpdateCustomerCommandHandler(ICustomerManager manager)
    {
        _manager = manager;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        await _manager.UpdateCustomerAsync(request.Customer,request.ModifiedBy, cancellationToken);
        return Unit.Value;
    }
}
