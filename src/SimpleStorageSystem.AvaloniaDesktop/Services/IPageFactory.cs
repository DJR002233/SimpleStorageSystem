using System;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.PageFactory;

public interface IPageFactory<T> where T : IPage
{
    T Create(Type pageType);
}
