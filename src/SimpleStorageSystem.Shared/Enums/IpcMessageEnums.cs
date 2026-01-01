namespace SimpleStorageSystem.Shared.Enums;

public enum IpcSender
{
    UserInterface,
    BackgroundService
}

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
    DisconnectStorageDrive,
}

public enum IpcStatus
{
    Ok,
    Failed,
    Error
}
