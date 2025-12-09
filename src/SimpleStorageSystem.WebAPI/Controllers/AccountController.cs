using Microsoft.AspNetCore.Mvc;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

            if (res.StatusMessage == ApiStatus.Success)
                return Ok(res);

            return Unauthorized(res);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

}
