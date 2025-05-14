using CustomerMergeAPI.Domain.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CustomerMergeAPI.Domain.Repositories.Interfaces;
using System.Data;
using System.Linq;
using System;
using CustomerMergeAPI.Domain.DTOs;
using System.Threading;

namespace CustomerMergeAPI.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IConfiguration _config;

    public CustomerRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<Customer>> GetDuplicateGroupsAsync(int page, int pageSize, string search, CancellationToken cancellationToken)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync(cancellationToken);
        var sql = @"
       WITH UnmergedGroupKeys AS (
    SELECT GroupKey
    FROM Customers
    WHERE ParentCustCode IS NULL
    GROUP BY GroupKey
    HAVING COUNT(*) > 1
),
Grouped AS (
    SELECT *, ROW_NUMBER() OVER (PARTITION BY GroupKey ORDER BY CustCode) AS rn
    FROM Customers
    WHERE GroupKey IN (SELECT GroupKey FROM UnmergedGroupKeys)
    AND (@Search IS NULL OR Name LIKE '%' + @Search + '%')
),
PagedKeys AS (
    SELECT DISTINCT GroupKey
    FROM Grouped
    ORDER BY GroupKey
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
)
SELECT * FROM Grouped
WHERE GroupKey IN (SELECT GroupKey FROM PagedKeys)

    ";

        return await conn.QueryAsync<Customer>(sql, new
        {
            Offset = (page - 1) * pageSize,
            PageSize = pageSize,
            Search = search
        });
    }



    public async Task<PagedCustomerResult> GetResolvedGroupsAsync(int page, int pageSize, string search, CancellationToken cancellationToken)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync(cancellationToken);
        var offset = (page - 1) * pageSize;

        var sql = @"
        WITH ParentCTE AS (
            SELECT *
            FROM Customers
            WHERE IsParent = 1
              AND (@Search IS NULL OR Name LIKE '%' + @Search + '%')
        ),
        PagedParents AS (
            SELECT *, COUNT(*) OVER() AS TotalCount
            FROM ParentCTE
            ORDER BY GroupKey
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
        )
        SELECT * FROM PagedParents;

        SELECT * FROM Customers
        WHERE ParentCustCode IS NOT NULL;
    ";

        using var multi = await conn.QueryMultipleAsync(sql, new
        {
            Offset = offset,
            PageSize = pageSize,
            Search = string.IsNullOrWhiteSpace(search) ? null : search
        });

        var parents = (await multi.ReadAsync<Customer>()).ToList();
        var children = (await multi.ReadAsync<Customer>()).ToList();

        foreach (var parent in parents)
        {
            parent.Children = children.Where(c => c.ParentCustCode == parent.CustCode).ToList();
        }

        var totalCount = parents.FirstOrDefault()?.TotalCount ?? 0;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedCustomerResult
        {
            Data = parents,
            TotalPages = totalPages
        };
    }


    public async Task MergeGroupAsync(string groupKey, string parentCustCode, string mergedBy, CancellationToken cancellationToken)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync(cancellationToken);

        await conn.ExecuteAsync(
            "sp_MergeCustomerGroup",
            new { GroupKey = groupKey, ParentCustCode = parentCustCode, MergedBy = mergedBy },
            commandType: CommandType.StoredProcedure
        );

    }

    public async Task UpdateCustomerAsync(Customer customer,string modifiedBy, CancellationToken cancellationToken)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync(cancellationToken);
     
    var parameters = new
    {
        customer.CustCode,
        customer.Name,
        customer.Add01,
        customer.Add02,
        customer.PostCode,
        customer.Country,
        ModifiedUser = modifiedBy ?? "system"
    };

    await conn.ExecuteAsync("sp_UpdateCustomer", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> GetDuplicateGroupsCountAsync(string search, CancellationToken cancellationToken)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync(cancellationToken);
        var sql = @"
        WITH UnmergedGroupKeys AS (
            SELECT GroupKey
            FROM Customers
            WHERE ParentCustCode IS NULL
            GROUP BY GroupKey
            HAVING COUNT(*) > 1
        )
        SELECT COUNT(DISTINCT GroupKey)
        FROM Customers
        WHERE GroupKey IN (SELECT GroupKey FROM UnmergedGroupKeys)
          AND (@Search IS NULL OR Name LIKE '%' + @Search + '%')
    ";

        return await conn.ExecuteScalarAsync<int>(sql, new { Search = search });
    }

}
