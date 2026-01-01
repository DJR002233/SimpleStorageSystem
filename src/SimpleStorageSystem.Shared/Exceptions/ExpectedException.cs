namespace SimpleStorageSystem.Shared.Exceptions;

public class ExpectedException : Exception
{
    public ExpectedException(string message) : base (message) {}
}