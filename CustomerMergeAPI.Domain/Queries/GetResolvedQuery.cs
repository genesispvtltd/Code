using MediatR;
using CustomerMergeAPI.Domain.DTOs;

namespace CustomerMergeAPI.Domain.Queries;

public class GetResolvedQuery : IRequest<PagedCustomerResult>
{
    public int Page { get; }
    public int PageSize { get; }
    public string? Search { get; }

    public GetResolvedQuery(int page, int pageSize, string? search)
    {
        Page = page;
        PageSize = pageSize;
        Search = search;
    }
}
