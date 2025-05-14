using CustomerMergeAPI.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestQuery request, CancellationToken cancellationToken = default)
    {
        var token = await _mediator.Send(request, cancellationToken);
        if (string.IsNullOrEmpty(token))
            return Unauthorized("Invalid credentials");

        return Ok(new { token });
    }

}
