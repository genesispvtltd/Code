
using CustomerMergeAPI.Domain.Models;
using MediatR;


namespace CustomerMergeAPI.Domain.Commands;

public class MergeGroupCommand : IRequest<Unit>
{
    public string GroupKey { get; set; }
    public string ParentCustCode { get; set; }
    public string MergedBy { get; set; }
    public Customer? ParentCustomer { get; set; }
}
