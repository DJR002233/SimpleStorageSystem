using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.WebAPI.Data;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Services;

public class AccountService
{
    private readonly ApiDbContext _dbContext;
    private PasswordHasher<AccountInformation> _passwordHasher;
    public AccountService(ApiDbContext context, PasswordHasher<AccountInformation> passwordHasher)
    {
        _dbContext = context;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<ApiResponse> UpdateAccountAsync(string userId, UpdateAccountRequest request)
    {
        var account = _dbContext.Accounts.FirstOrDefault(a => a.UserId.ToString() == userId);
        if (account is null)
            return new ApiResponse { StatusCode = HttpStatusCode.NotFound, Message = "Account not found" };

        bool emailExists = await _dbContext.Accounts.AnyAsync(a => a.Email == request.Email);

        if (emailExists)
            return new ApiResponse { StatusCode = HttpStatusCode.Conflict, Message = "Email is already taken!" };

        if (!String.IsNullOrWhiteSpace(request.Username) && !account.Username!.Equals(request.Username))
            account.Username = request.Username;
        if (!String.IsNullOrWhiteSpace(request.Email))
            account.Email = request.Email;
        if (!String.IsNullOrWhiteSpace(request.Password))
        {
            var passwordIsEqual = _passwordHasher.VerifyHashedPassword(new AccountInformation { }, account.Password!, request.Password);
            if (passwordIsEqual == PasswordVerificationResult.Failed)
            {
                request.Password = _passwordHasher.HashPassword(account, request.Password!);
                account.Password = request.Password;
            }
        }

        int rowAffected = await _dbContext.SaveChangesAsync();
        if (rowAffected > 0) return new ApiResponse { StatusCode = HttpStatusCode.OK, Message = "Account Information Updated!" };

        return new ApiResponse { StatusCode = HttpStatusCode.InternalServerError, Message = "Database Problem!\n\nUpdate Failed" };
    }

}
