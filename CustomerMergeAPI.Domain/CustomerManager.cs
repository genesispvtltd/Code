using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomerMergeAPI.Domain.DTOs;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.Domain.Models;
using CustomerMergeAPI.Domain.Repositories.Interfaces;

namespace CustomerMergeAPI.Data;

public class CustomerManager : ICustomerManager
{
    private readonly ICustomerRepository _repo;

    public CustomerManager(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<Customer>> GetDuplicateGroupsAsync(int page, int pageSize, string search, CancellationToken cancellationToken)
        => _repo.GetDuplicateGroupsAsync(page, pageSize, search, cancellationToken);

    public Task<PagedCustomerResult> GetResolvedGroupsAsync(int page, int pageSize, string search, CancellationToken cancellationToken)
        => _repo.GetResolvedGroupsAsync(page, pageSize, search, cancellationToken);

    public Task MergeGroupAsync(string groupKey, string parentCustCode, string mergedBy, CancellationToken cancellationToken)
        => _repo.MergeGroupAsync(groupKey, parentCustCode, mergedBy, cancellationToken);

    public Task<int> GetDuplicateGroupsCountAsync(string search, CancellationToken cancellationToken)
    => _repo.GetDuplicateGroupsCountAsync(search, cancellationToken);

    public Task UpdateCustomerAsync(Customer customer,string modifiedBy, CancellationToken cancellationToken)
    {
        return _repo.UpdateCustomerAsync(customer,modifiedBy, cancellationToken);
    }
}

