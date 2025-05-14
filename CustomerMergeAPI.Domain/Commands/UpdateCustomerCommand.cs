using CustomerMergeAPI.Domain.Models;
using MediatR;

public class UpdateCustomerCommand : IRequest<Unit>
{
    public Customer Customer { get; set; }
    public string ModifiedBy{get;set;}
}