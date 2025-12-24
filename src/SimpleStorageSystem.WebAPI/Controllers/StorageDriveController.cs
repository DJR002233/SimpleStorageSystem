using Microsoft.AspNetCore.Mvc;
using SimpleStorageSystem.WebAPI.Services;

namespace SimpleStorageSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageDriveController : Controller
{
    private readonly StorageDriveServices _storageDrive;

    public StorageDriveController(StorageDriveServices storageDrive)
    {
        _storageDrive = storageDrive;
    }

    [HttpGet("get_drives")]
    public async ValueTask<IActionResult> GetUserStorageDrives()
    {
        await Task.Delay(1000);
        return Problem("Unimplemented");
    }

    [HttpPost("create_drive")]
    public async ValueTask<IActionResult> AddUserStorageDrive()
    {
        await Task.Delay(1000);
        return Problem("Unimplemented");
    }

    [HttpPut("rename_drive")]
    public async ValueTask<IActionResult> RenameUserStorageDrive()
    {
        await Task.Delay(1000);
        return Problem("Unimplemented");
    }

    [HttpDelete("delete_drive")]
    public async ValueTask<IActionResult> DeleteUserStorageDrive()
    {
        await Task.Delay(1000);
        return Problem("Unimplemented");
    }
}