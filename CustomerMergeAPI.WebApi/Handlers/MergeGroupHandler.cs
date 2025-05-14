using MediatR;
using CustomerMergeAPI.Domain.Commands;
using CustomerMergeAPI.Domain.Interfaces;

namespace CustomerMergeAPI.WebApi.Handlers;

public class MergeGroupHandler : IRequestHandler<MergeGroupCommand, Unit>
{
    private readonly ICustomerManager _manager;

    public MergeGroupHandler(ICustomerManager manager)
    {
        _manager = manager;
    }

    public async Task<Unit> Handle(MergeGroupCommand request, CancellationToken cancellationToken)
    {
        await _manager.MergeGroupAsync(request.GroupKey, request.ParentCustCode, request.MergedBy, cancellationToken);
        return Unit.Value;
    }
}
