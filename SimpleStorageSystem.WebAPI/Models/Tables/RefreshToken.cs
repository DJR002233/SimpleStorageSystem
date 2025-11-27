namespace SimpleStorageSystem.WebAPI.Models.Tables;

public class RefreshToken
{
    public long Id { get; set; }
    public string? Token { get; set; }
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
    //public bool Used { get; set; } = false;
    public bool Revoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ReplacedByToken { get; set; }

    public Guid UserId { get; set; }
    public AccountInformation Account { get; set; } = null!;
    // public byte[] RowVersion { get; set; } = null!;
}
