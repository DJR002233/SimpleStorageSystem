namespace SimpleStorageSystem.Shared.Services.Helper;

public class AdjustableSemaphoreSlim
{
    private SemaphoreSlim _semaphore;

    public AdjustableSemaphoreSlim(int initialCount)
    {
        _semaphore = new SemaphoreSlim(initialCount);
    }

    public async ValueTask WaitAsync() => await _semaphore.WaitAsync();
    
    public void Release() => _semaphore.Release();

    public void UpdateLimit(int newLimit)
    {
        var old = _semaphore;
        _semaphore = new SemaphoreSlim(newLimit);
        old.Dispose();
    }
}