using System.ComponentModel.DataAnnotations;

namespace SimpleStorageSystem.Shared.Requests;

public class UpdateAccountRequest
{
    public string? Username { get; set; }
    // [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
    
}
