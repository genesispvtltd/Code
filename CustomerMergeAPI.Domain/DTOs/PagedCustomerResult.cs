using System.Collections.Generic;
using CustomerMergeAPI.Domain.Models;

namespace CustomerMergeAPI.Domain.DTOs;

public class PagedCustomerResult
{
    public List<Customer> Data { get; set; }
    public int TotalPages { get; set; }
    public string? BannerMessage { get; set; }
    public string? BannerType { get; set; }
}
