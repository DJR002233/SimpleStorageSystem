using System;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.PageFactory;

public interface IPageFactory<T>
{
    T Create(Type pageType);
}
