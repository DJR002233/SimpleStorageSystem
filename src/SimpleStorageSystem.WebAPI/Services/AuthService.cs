using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SimpleStorageSystem.WebAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleStorageSystem.WebAPI.Models.Tables;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Helper;

namespace SimpleStorageSystem.WebAPI.Services;

public class AuthService
{
    private readonly ApiDbContext _dbContext;
    private IConfiguration _configuration;
    private PasswordHasher<AccountInformation> _passwordHasher;

    public AuthService(ApiDbContext context, IConfiguration configuration, PasswordHasher<AccountInformation> passwordHasher)
    {
        _dbContext = context;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<Session>> LoginAccountAsync(LoginRequest user)
    {
        var invalidResponse = CreateApiResponse.Failed<Session>("Invalid Credentials!");

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(b => b.Email == user.Email);
        if (account is null || account.UserId is null)
            return invalidResponse;

        var passwordResult = _passwordHasher.VerifyHashedPassword(new AccountInformation { Email = user.Email!, Password = user.Password! }, account.Password!, user.Password!);
        if (passwordResult != PasswordVerificationResult.Success)
            return invalidResponse;

        string dbToken = GenerateRefreshToken();
        AccessToken jwtToken = GenerateAccessToken(account.UserId.Value, account.Email!);

        account.Token.Add(new RefreshToken { Token = dbToken, UserId = account.UserId.Value });
        await _dbContext.SaveChangesAsync();

        return CreateApiResponse.Success(
            new Session
            {
                AccessToken = new AccessToken
                {
                    Token = jwtToken.Token,
                    Expiration = jwtToken.Expiration
                },
                RefreshToken = dbToken
            }
        );
    }

    public async Task<ApiResponse> CreateAccountAsync(AccountInformation user)
    {
        bool isDisabled = false;
        if(isDisabled)
            return CreateApiResponse.Unauthorized("Account creation is disabled");
            
        user.Password = _passwordHasher.HashPassword(user, user.Password!);
        bool emailExists = await _dbContext.Accounts.AnyAsync(b => b.Email == user.Email);

        if (emailExists)
            return CreateApiResponse.Failed("Email is already taken!");

        _dbContext.Accounts.Add(user);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected > 0)
            return CreateApiResponse.Success("Account Created!");

        return CreateApiResponse.Error("Failed to create account!");
    }

    public async Task<ApiResponse<Session>> GetAccessTokenAsync(string refreshToken)
    {
        var rfToken = await _dbContext.Tokens.Include(t => t.Account).FirstOrDefaultAsync(t => t.Token == refreshToken);
        if (rfToken is null)
            return CreateApiResponse.Failed<Session>("Token Not Found!");

        if (rfToken.Revoked)
        {
            // await RevokeAllUserTokensAsync(rfToken.UserId);
            return CreateApiResponse.Failed<Session>("Invalid Token!\nPlease login again...");
        }

        if (rfToken.ExpiresAt <= DateTime.UtcNow)
        {
            rfToken.Revoked = true;
            await _dbContext.SaveChangesAsync();
            return CreateApiResponse.Failed<Session>("Token Expired!\nPlease login again...");
        }

        string newRefreshToken = GenerateRefreshToken();

        rfToken.Revoked = true;
        rfToken.ReplacedByToken = newRefreshToken;

        _dbContext.Tokens.Add(new RefreshToken { Token = newRefreshToken, UserId = rfToken.UserId });

        AccessToken accessToken = GenerateAccessToken(rfToken.UserId, rfToken.Account.Email!);

        try
        {
            int rowsAffected = await _dbContext.SaveChangesAsync();

            if (rowsAffected <= 0)
            {
                await RevokeAllUserTokensAsync(rfToken.UserId);
                return CreateApiResponse.Failed<Session>("Failed to create token!");
            }

            return CreateApiResponse.Success(
                new Session
                {
                    AccessToken = new AccessToken
                    {
                        Token = accessToken.Token,
                        Expiration = accessToken.Expiration
                    },
                    RefreshToken = newRefreshToken
                }
            );

        } catch (DbUpdateConcurrencyException)
        {
            // This happens if EF detects someone else modified the same record
            // await RevokeAllUserTokensAsync(token.UserId);
            // return false;
            await RevokeAllUserTokensAsync(rfToken.UserId);
            throw;
            // return new ApiResponse<Session>
            // {
            //     Title = ex.GetType().ToString(),
            //     StatusMessage = ApiStatus.Error,
            //     Message = "Token Has been modified"
            // };
        }/* catch (DbUpdateException) {
            // General database failure (constraint violation, FK error, etc.)
            // await RevokeAllUserTokensAsync(token.UserId);
            // return false;
            return new Response<Session>
            {
                StatusMessage = "Failed",
                Message = "Database Error!"
            };
        } catch (Exception ex) {
            // Log for diagnostics
            //_logger.LogError(ex, "Unexpected error saving token.");
            // await RevokeAllUserTokensAsync(token.UserId);
            // return false;
            return new Response<Session>
            {
                StatusMessage = "Failed",
                Message = ex.Message
            };
        }/**/

    }

    public async Task<ApiResponse> ClearTokenAsync(string? refreshToken)
    {
        var rfToken = await _dbContext.Tokens.Include(t => t.Account).FirstOrDefaultAsync(t => t.Token == refreshToken);
        if (rfToken is null)
            return CreateApiResponse.Failed("Token Not Found!");

        if (rfToken.Revoked)
        {
            //await RevokeAllUserTokensAsync(rfToken.UserId);
            return CreateApiResponse.Failed("Invalid Token!\nPlease login again...");
        }

        rfToken.Revoked = true;

        try
        {
            int rowsAffected = await _dbContext.SaveChangesAsync();

            if (rowsAffected <= 0)
            {
                await RevokeAllUserTokensAsync(rfToken.UserId);
                return CreateApiResponse.Failed("Token already revoked!\nRevoked all live tokens!");
            }

            if (rfToken.ExpiresAt <= DateTime.UtcNow)
                return CreateApiResponse.Failed("Token Expired!\nPlease login again...");

            return CreateApiResponse.Success();
        } catch (DbUpdateConcurrencyException)
        {
            // This happens if EF detects someone else modified the same record
            // await RevokeAllUserTokensAsync(token.UserId);
            // return false;
            await RevokeAllUserTokensAsync(rfToken.UserId);
            throw;
            // return new ApiResponse<Session>
            // {
            //     Title = ex.GetType().ToString(),
            //     StatusMessage = ApiStatus.Error,
            //     Message = "Token Has been modified"
            // };
        }
    }

    public AccessToken GenerateAccessToken(Guid id, string email)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", "User")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var tokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: creds
        );
        return new AccessToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            Expiration = tokenExpiration
        };
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        string token = Convert.ToBase64String(bytes);
        return token;
    }

    public async Task<bool> RevokeAllUserTokensAsync(Guid userId)
    {
        var tokens = _dbContext.Tokens.Where(t => t.UserId == userId).ToList();
        foreach (var token in tokens)
            token.Revoked = true;
        return await _dbContext.SaveChangesAsync() > 0;
    }

    /*
        // Read
        public async Task<List<AccountInformation>> GetAccountAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        // Update
        public async Task<bool> UpdateAccountAsync(AccountInformation user)
        {
            _context.Accounts.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        // Delete
        public async Task<bool> DeleteAccountAsync(int id)
        {
            var user = await _context.Accounts.FindAsync(id);
            if (user == null) return false;

            _context.Accounts.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    */

}
