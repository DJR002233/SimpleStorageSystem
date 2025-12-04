namespace SimpleStorageSystem.Shared.Models;

public class AccessToken
{
    private string? token;
    private DateTime? expiration;

    public string? Token
    {
        get => token;
        set => token = value;
    }

    public DateTime? Expiration
    {
        get => expiration;
        set => expiration = value;
    }

    public void Set(string? token, DateTime? expiration)
    {
        this.token = token;
        this.expiration = expiration;
    }

    public void SetNew(AccessToken accessToken)
    {
        token = accessToken.Token;
        expiration = accessToken.Expiration;
    }

    public void Clear()
    {
        token = null;
        expiration = null;
    }

}
