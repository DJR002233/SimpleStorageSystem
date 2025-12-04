namespace SimpleStorageSystem.Shared.Requests;

public class CreateAccountRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

}
