using System;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler;

public class OnUnauthorizedHandler
{
    public event Action? OnUnauthorized;

    public void TriggerUnauthorized() => OnUnauthorized?.Invoke();
}
