using CustomerMergeAPI.Domain.Commands;
using CustomerMergeAPI.Domain.Constants;
using CustomerMergeAPI.Domain.DTOs;
using CustomerMergeAPI.Domain.Models;
using CustomerMergeAPI.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CompanyAdmin,ClientSpecialist")]
public class CustomerController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet("duplicates")]
    public async Task<IActionResult> GetDuplicates([FromQuery] int page = 1, [FromQuery] int pageSize = 10, string? search = null, CancellationToken cancellationToken = default)
    {

        try
        {
            var result = await _mediator.Send(new GetDuplicatesQuery { Page = page, PageSize = pageSize, Search = search }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving duplicates. Page: {Page}, PageSize: {PageSize}, Search: {Search}",
                     page, pageSize, search);

            return StatusCode(500, "An error occurred while retrieving duplicates.");


        }
    }

    [HttpGet("resolved")]
    public async Task<IActionResult> GetResolved([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _mediator.Send(new GetResolvedQuery(page, pageSize, search), cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
           "Error occurred while retrieving resolved customer groups. Page: {Page}, PageSize: {PageSize}, Search: {Search}",
           page, pageSize, search);

            return StatusCode(500, "An error occurred while retrieving resolved customer groups.");
        }
    }

    [HttpPost("merge")]
    public async Task<IActionResult> MergeGroup([FromBody] MergeGroupCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            command.MergedBy = username ?? "system";

            await _mediator.Send(command, cancellationToken);
            return Ok(new PagedCustomerResult
            {
                BannerMessage = BannerMessages.SUCCESS_MERGE,
                BannerType = BannerMessageTypes.SUCCESS
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error merging group {GroupKey}", command.GroupKey);
            return BadRequest(new PagedCustomerResult
            {
                BannerMessage = BannerMessages.ERROR_MERGE + ": " + ex.Message,
                BannerType = BannerMessageTypes.ERROR
            });
        }
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCustomer([FromBody] Customer customer, CancellationToken cancellationToken = default)
    {
        try
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            username = username ?? "system";
            await _mediator.Send(new UpdateCustomerCommand { Customer = customer, ModifiedBy = username }, cancellationToken);
            return Ok(new PagedCustomerResult
            {
                BannerMessage = BannerMessages.SUCCESS_UPDATE,
                BannerType = BannerMessageTypes.SUCCESS
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving customer {CustCode}", customer.CustCode);
            return BadRequest(new PagedCustomerResult
            {
                BannerMessage = BannerMessages.ERROR_SAVE + ": " + ex.Message,
                BannerType = BannerMessageTypes.ERROR
            });
        }

    }

}
