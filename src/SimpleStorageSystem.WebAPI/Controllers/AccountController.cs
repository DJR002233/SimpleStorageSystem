using Microsoft.AspNetCore.Mvc;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net;

namespace SimpleStorageSystem.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    
    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAccountInformation([FromBody] UpdateAccountRequest request)
    {
        try
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("Invalid token");

            ApiResponse res = await _accountService.UpdateAccountAsync(userId, request);

            if (res.StatusCode == HttpStatusCode.NoContent)
                return Ok(res);
            else if (res.StatusCode == HttpStatusCode.Conflict)
                return Conflict(res);
            else if (res.StatusCode == HttpStatusCode.NotFound)
                return NotFound(res);

            throw new Exception("Response not sent");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

}
