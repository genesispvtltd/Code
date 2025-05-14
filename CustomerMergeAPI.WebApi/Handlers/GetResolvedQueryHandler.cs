using MediatR;
using CustomerMergeAPI.Domain.Queries;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.Domain.DTOs;


namespace CustomerMergeAPI.WebApi.Handlers;

public class GetResolvedQueryHandler : IRequestHandler<GetResolvedQuery, PagedCustomerResult>
{
    private readonly ICustomerManager _manager;

    public GetResolvedQueryHandler(ICustomerManager manager)
    {
        _manager = manager;
    }

    public async Task<PagedCustomerResult> Handle(GetResolvedQuery request, CancellationToken cancellationToken)
    {
        var result = await _manager.GetResolvedGroupsAsync(request.Page, request.PageSize, request.Search ?? string.Empty, cancellationToken);
        return result;
    }
}