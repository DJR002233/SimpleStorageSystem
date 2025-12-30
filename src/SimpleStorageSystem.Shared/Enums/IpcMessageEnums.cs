namespace SimpleStorageSystem.Shared.Enums;

public enum IpcType
{
    Request,
    Response
}

public enum IpcCommand
{
    Login,
    CreateAccount,
    HasSession,
    Logout,
    UpdateAccount,
    GetStorageDriveList,
    AddStorageDrive,
    RenameStorageDrive,
    DeleteStorageDrive,
}

public enum IpcStatus
{
    Ok,
    Failed,
    Error
}
