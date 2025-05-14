namespace CustomerMergeAPI.Domain.Models;

public class MergeRequest
{
    public string GroupKey { get; set; } = string.Empty;
    public string ParentCustCode { get; set; } = string.Empty;
}
