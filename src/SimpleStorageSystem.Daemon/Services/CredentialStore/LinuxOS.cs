using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;

public class LinuxSecretToolStore : ICredentialStore
{
    private const string AttrLabel = "SimpleStorageDesktop";
    private const string AttrKey = "StorageDriveId";

    private static bool IsSecretToolAvailable()
    {
        try
        {
            var p = Process.Start(new ProcessStartInfo("which", "secret-tool")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            p?.WaitForExit();
            return p?.ExitCode == 0;
        }
        catch { return false; }
    }

    public async ValueTask StoreAsync(string password, string value)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            throw new PlatformNotSupportedException();

        if (!IsSecretToolAvailable())
            throw new InvalidOperationException("secret-tool is not installed on this machine.");

        //string json = JsonSerializer.Serialize(session.RefreshToken);

        // secret-tool store app <app> type <type>
        var psi = new ProcessStartInfo("secret-tool",
            $"store --label='{AttrLabel}' {AttrKey} {value}")
        {
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var p = Process.Start(psi) ?? throw new InvalidOperationException("failed to start secret-tool");
        await p.StandardInput.WriteAsync(password);
        p.StandardInput.Close();
        await p.WaitForExitAsync();
        if (p.ExitCode != 0)
            throw new InvalidOperationException($"secret-tool store failed with exit code {p.ExitCode}");
    }

    public async ValueTask<string?> GetAsync(string value)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            throw new PlatformNotSupportedException();

        if (!IsSecretToolAvailable())
            return null;

        var psi = new ProcessStartInfo("secret-tool",
            $"lookup {AttrKey} {value}")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var p = Process.Start(psi) ?? throw new InvalidOperationException("failed to start secret-tool");
        string output = await p.StandardOutput.ReadToEndAsync();
        await p.WaitForExitAsync();
        if (p.ExitCode != 0 || string.IsNullOrWhiteSpace(output))
            return null;

        try
        {
            return output.Trim();
        }
        catch
        {
            return null;
        }
    }

    public async ValueTask DeleteAsync(string value)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            throw new PlatformNotSupportedException();

        if (!IsSecretToolAvailable())
            throw new InvalidOperationException("secret-tool is not installed on this machine.");

        var psi = new ProcessStartInfo("secret-tool",
            $"clear {AttrKey} {value}")
        {
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var p = Process.Start(psi) ?? throw new InvalidOperationException("failed to start secret-tool");
        await p.WaitForExitAsync();
    }
    
}
