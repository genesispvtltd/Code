using CustomerMergeAPI.Domain.DTOs;
using MediatR;

namespace CustomerMergeAPI.Domain.Queries;
public class GetDuplicatesQuery : IRequest<PagedCustomerResult>
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public string Search { get; set; }
}