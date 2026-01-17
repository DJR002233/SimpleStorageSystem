using System;

namespace SimpleStorageSystem.AvaloniaDesktop.Services;

public interface IPage
{
    string? PageName { get; }
    Type PageType { get; }
}
