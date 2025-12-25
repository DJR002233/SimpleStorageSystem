using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SimpleStorageSystem.WebAPI.Models.Tables;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.WebAPI.Services;
using SimpleStorageSystem.Shared.Services.Helper;
using System.Net;

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

            if (res.StatusCode == HttpStatusCode.OK && res.Data is not null)
                return Ok(res);
            else if (res.StatusCode == HttpStatusCode.BadRequest) return ValidationProblem();

            throw new Exception("Response not sent");
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

            if (res.StatusCode == HttpStatusCode.OK) return Ok(res);
            else if (res.StatusCode == HttpStatusCode.Forbidden) return Forbid();
            else if (res.StatusCode == HttpStatusCode.Conflict) return Conflict(res);
            else if (res.StatusCode == HttpStatusCode.InternalServerError) return Problem();

            throw new Exception("Response not sent");
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

            if (res.StatusCode == HttpStatusCode.OK) return Ok(res);
            if (res.StatusCode == HttpStatusCode.BadRequest) return BadRequest(res);
            if (res.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized(res);
            if (res.StatusCode == HttpStatusCode.Conflict) return Conflict(res);

            throw new Exception("Response not sent");
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

            if (res.StatusCode == HttpStatusCode.NoContent) return NoContent();
            else if (res.StatusCode == HttpStatusCode.BadRequest) return BadRequest(res);
            else if (res.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized(res);
            else if (res.StatusCode == HttpStatusCode.Conflict) return Conflict(res);

            throw new Exception("Response not sent");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

}
