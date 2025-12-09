using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Helper;
using SimpleStorageSystem.WebAPI.Data;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Services;

public class AccountService
{
    private readonly MyDbContext _context;
    private PasswordHasher<AccountInformation> _passwordHasher;
    public AccountService(MyDbContext context, PasswordHasher<AccountInformation> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse> UpdateAccountAsync(string userId, UpdateAccountRequest request)
    {
        var account = _context.Accounts.FirstOrDefault( a => a.UserId.ToString() == userId);
        if (account is null)
            return CreateApiResponse.Unauthorized("Invalid Account");
        
        bool emailExists = await _context.Accounts.AnyAsync(a => a.Email == request.Email);
        if (emailExists)
            return CreateApiResponse.Failed("Email is already taken!");

        request.Password = _passwordHasher.HashPassword(account, request.Password!);

        if (!String.IsNullOrWhiteSpace(request.Username))
            account.Username = request.Username;
        if (!String.IsNullOrWhiteSpace(request.Email))
            account.Email = request.Email;
        if (!String.IsNullOrWhiteSpace(request.Password))
            account.Password = request.Password;

        int rowAffected = await _context.SaveChangesAsync();
        if (rowAffected > 0) return CreateApiResponse.Success();

        return CreateApiResponse.Error("Database Problem!", "Update Failed");
    }
}