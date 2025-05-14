using System.Collections.Generic;

namespace CustomerMergeAPI.Domain.Models;

public class Customer
{
    public string CustCode { get; set; }
    public string Name { get; set; }
    public string? Add01 { get; set; }
    public string? Add02 { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string GroupKey { get; set; }
    public bool IsParent { get; set; }
    public string? ParentCustCode { get; set; }
    public string? MergedBy { get; set; }


    public int TotalCount { get; set; }

    public List<Customer> Children { get; set; } = new();
}

