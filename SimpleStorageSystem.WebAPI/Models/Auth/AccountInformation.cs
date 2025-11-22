namespace SimpleStorageSystem.WebAPI.Models.Auth;

public class AccountInformation
{
    public Guid? UserId { get; set; }
    public long? Id { get; set; }
    public string? Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    //public AccountConfiguration AccountData { get; set; } = null!;
    public ICollection<RefreshToken> Token { get; set; } = new List<RefreshToken>();
}
