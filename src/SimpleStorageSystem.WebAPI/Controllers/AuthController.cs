using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SimpleStorageSystem.WebAPI.Services.Auth;
using SimpleStorageSystem.WebAPI.Models.Tables;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Services.Helper;

namespace SimpleStorageSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AccountService _accountService;
    public AuthController(AccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // if (request.AnyPropertyIsNullorWhiteSpace())
        //     return Unauthorized(CreateApiResponse.Failed("Invalid Credentials!"));
        ApiResponse<Session> res = await _accountService.LoginAccountAsync(request);
        if (res.StatusMessage == ApiStatus.Success && !String.IsNullOrWhiteSpace(res?.Data?.ToString()))
            return Ok(res);

        return Unauthorized(res);
    }

    [HttpPost("sign_up")]
    public async Task<IActionResult> AddAccount([FromBody] CreateAccountRequest request)
    {
        // if (request.AnyPropertyIsNullorWhiteSpace())
        //     return BadRequest(CreateApiResponse.Failed("Missing Values!"));

        AccountInformation account = new AccountInformation { Username = request.Username, Email = request.Email, Password = request.Password };

        ApiResponse res = await _accountService.CreateAccountAsync(account);

        if (res.StatusMessage == ApiStatus.Success) return Ok(res);
        else if (res.StatusMessage == ApiStatus.Failed) return Conflict(res);

        return Forbid();
    }

    [HttpGet("refresh_session")]
    public async Task<IActionResult> RenewToken([FromHeader(Name = "X-Refresh-Token")] string? refreshToken)
    {
        if (String.IsNullOrWhiteSpace(refreshToken))
            return BadRequest(new { Message = "Missing Token Header" });
        //return Ok(new { StatusMessage = "Error", Message = refreshToken });
        //int accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ApiResponse<Session> res = await _accountService.GetAccessTokenAsync(refreshToken);
        //Console.WriteLine(res.Message);
        //return Ok(res);
        //Thread.Sleep(10000);
        if (res.StatusMessage == ApiStatus.Success)
            return Ok(res);

        return Unauthorized(res);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout([FromHeader(Name = "X-Refresh-Token")] string? refreshToken)
    {
        if (String.IsNullOrWhiteSpace(refreshToken))
            return BadRequest(new { Message = "Missing Token Header" });
        ApiResponse res = await _accountService.ClearTokenAsync(refreshToken);
        if (res.StatusMessage == ApiStatus.Success)
            return Ok(res);

        return Unauthorized(res);/**/
    }

}
