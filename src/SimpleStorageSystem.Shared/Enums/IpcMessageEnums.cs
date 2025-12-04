namespace SimpleStorageSystem.Shared.Enums;

public enum IpcType
{
    Request,
    Response
}

public enum IpcCommand
{
    LOGIN,
    CREATE_ACCOUNT,
    HAS_SESSION,
    LOGOUT,
    UPDATE_ACCOUNT,
}

public enum IpcStatus
{
    OK,
    FAILED,
    ERROR
}
