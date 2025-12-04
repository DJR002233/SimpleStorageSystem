namespace SimpleStorageSystem.WebAPI.Models.Tables;

public class AccountInformation
{
    public Guid? UserId { get; set; }
    // public long? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RefreshToken> Token { get; set; } = new List<RefreshToken>();
    
}
