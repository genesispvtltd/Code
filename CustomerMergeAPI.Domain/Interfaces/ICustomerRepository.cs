using CustomerMergeAPI.Domain.DTOs;
using CustomerMergeAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerMergeAPI.Domain.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetDuplicateGroupsAsync(int page, int pageSize, string search, CancellationToken cancellationToken);
        Task<PagedCustomerResult> GetResolvedGroupsAsync(int page, int pageSize, string search, CancellationToken cancellationToken);
        Task MergeGroupAsync(string groupKey, string parentCustCode, string mergedBy, CancellationToken cancellationToken);
        Task UpdateCustomerAsync(Customer customer,string modifiedBy, CancellationToken cancellationToken);
        Task<int> GetDuplicateGroupsCountAsync(string search, CancellationToken cancellationToken);
    }
}