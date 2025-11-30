using System;

namespace SimpleStorageSystem.Daemon.Models;

public class Session
{
    private string? accessToken;
    public DateTime? Expiration { get; set; }
    public string? RefreshToken { get; set; }

    public string? AccessToken
    {
        get
        {
            if (
                Expiration is not null &&
                !String.IsNullOrWhiteSpace(accessToken) &&
                Expiration > DateTime.UtcNow &&
                !String.IsNullOrWhiteSpace(RefreshToken)
            )
                return accessToken;
            return null;
        }

        set => accessToken = value;
    }

}
