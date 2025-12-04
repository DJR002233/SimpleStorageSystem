using System.Threading.Tasks;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Main;

public class AccountService
{

    public AccountService()
    {
        
    }

    public async Task<IpcResponse> UpdateAccountInformation(string? username, string? email, string? password)
    {
        await Task.Delay(1000);
        return new IpcResponse{};
    }
    
}
