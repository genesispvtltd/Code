using MediatR;
using CustomerMergeAPI.Domain.Queries;
using CustomerMergeAPI.Domain.Models;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.Domain.DTOs;

namespace CustomerMergeAPI.WebApi.Handlers;

public class GetDuplicatesQueryHandler : IRequestHandler<GetDuplicatesQuery, PagedCustomerResult>
{
    private readonly ICustomerManager _manager;

    public GetDuplicatesQueryHandler(ICustomerManager manager)
    {
        _manager = manager;
    }

    public async Task<PagedCustomerResult> Handle(GetDuplicatesQuery request, CancellationToken cancellationToken)
    {
        var customers = await _manager.GetDuplicateGroupsAsync(request.Page, request.PageSize, request.Search, cancellationToken);
        var totalCount = await _manager.GetDuplicateGroupsCountAsync(request.Search, cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PagedCustomerResult
        {
            Data = customers.ToList(),
            TotalPages = totalPages
        };

    }
}
