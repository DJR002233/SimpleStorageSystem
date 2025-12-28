using System.ComponentModel.DataAnnotations;

namespace SimpleStorageSystem.Shared.Requests;

public class LoginRequest
{
    // [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }

}
