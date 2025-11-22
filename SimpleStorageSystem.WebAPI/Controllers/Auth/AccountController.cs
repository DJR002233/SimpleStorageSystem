using Microsoft.AspNetCore.Mvc;
using SimpleStorageSystem.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using SimpleStorageSystem.WebAPI.Requests.Auth;
using SimpleStorageSystem.WebAPI.Models.Auth;
using SimpleStorageSystem.WebAPI.Services.Auth;
using SimpleStorageSystem.WebAPI.Services.Helper;

namespace SimpleStorageSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountService _accountService;
    public AccountsController(AccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request.AnyPropertyIsNullorWhiteSpace())
            return Unauthorized(CreateResponse.Failed("Invalid Credentials!"));
        Response<Session> res = await _accountService.LoginAccountAsync(request);
        if (res.StatusMessage == StatusMessage.Success && !String.IsNullOrWhiteSpace(res?.Data?.ToString()))
            return Ok(res);

        return Unauthorized(res);
    }

    [HttpPost("sign_up")]
    public async Task<IActionResult> AddAccount([FromBody] CreateAccountRequest request)
    {
        if (request.AnyPropertyIsNullorWhiteSpace())
            return BadRequest(CreateResponse.Failed("Missing Values!"));

        AccountInformation account = new AccountInformation { Username = request.Username, Email = request.Email!, Password = request.Password! };

        Response res = await _accountService.CreateAccountAsync(account);

        if (res.StatusMessage == StatusMessage.Success) return Ok(res);
        else if (res.StatusMessage == StatusMessage.Failed) return Conflict(res);

        return Forbid();
    }

    [HttpGet("renew_token")]
    public async Task<IActionResult> RenewToken([FromHeader(Name = "X-Refresh-Token")] string? refreshToken)
    {
        if (String.IsNullOrWhiteSpace(refreshToken))
            return BadRequest(new { Message = "Missing Token Header" });
        //return Ok(new { StatusMessage = "Error", Message = refreshToken });
        //int accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        Response<Session> res = await _accountService.GetAccessTokenAsync(refreshToken);
        //Console.WriteLine(res.Message);
        //return Ok(res);
        //Thread.Sleep(10000);
        if (res.StatusMessage == StatusMessage.Success)
            return Ok(res);

        return Unauthorized(res);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout([FromHeader(Name = "X-Refresh-Token")] string? refreshToken)
    {
        if (String.IsNullOrWhiteSpace(refreshToken))
            return BadRequest(new { Message = "Missing Token Header" });
        Response res = await _accountService.ClearTokenAsync(refreshToken);
        if (res.StatusMessage == StatusMessage.Success)
            return Ok(res);

        return Unauthorized(res);/**/
    }

}
