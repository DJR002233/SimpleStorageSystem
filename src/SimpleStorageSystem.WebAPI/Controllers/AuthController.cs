using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SimpleStorageSystem.WebAPI.Models.Tables;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.WebAPI.Services;
using SimpleStorageSystem.Shared.Services.Helper;

namespace SimpleStorageSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // if (request.AnyPropertyIsNullorWhiteSpace())
        //     return Unauthorized(CreateApiResponse.Failed("Invalid Credentials!"));

        try
        {
            ApiResponse<Session> res = await _authService.LoginAccountAsync(request);

            if (res.StatusMessage == ApiStatus.Success && !String.IsNullOrWhiteSpace(res?.Data?.ToString()))
                return Ok(res);

            return Unauthorized(res);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

    [HttpPost("sign_up")]
    public async Task<IActionResult> AddAccount([FromBody] CreateAccountRequest request)
    {
        // if (request.AnyPropertyIsNullorWhiteSpace())
        //     return BadRequest(CreateApiResponse.Failed("Missing Values!"));

        try
        {
            AccountInformation account = new AccountInformation { Username = request.Username, Email = request.Email, Password = request.Password };

            ApiResponse res = await _authService.CreateAccountAsync(account);

            if (res.StatusMessage == ApiStatus.Success) return Ok(res);
            else if (res.StatusMessage == ApiStatus.Failed) return Conflict(res);

            return Forbid(res.Message!);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

    [HttpGet("refresh_session")]
    public async Task<IActionResult> RenewToken([FromHeader(Name = "X-Refresh-Token")] string? refreshToken)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(CreateApiResponse.Failed("Missing Token Header"));

            ApiResponse<Session> res = await _authService.GetAccessTokenAsync(refreshToken);

            if (res.StatusMessage == ApiStatus.Success) return Ok(res);

            return Unauthorized(res);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout([FromHeader(Name = "X-Refresh-Token")] string? refreshToken)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(CreateApiResponse.Failed("Missing Token Header"));

            ApiResponse res = await _authService.ClearTokenAsync(refreshToken);

            if (res.StatusMessage == ApiStatus.Success) return Ok(res);

            return Unauthorized(res);/**/
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

}
