using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

public class MessageDialogBoxViewModel : ReactiveObject
{
    #region Commands
    public ReactiveCommand<Unit, bool> ConfirmCommand { get; }
    public ReactiveCommand<Unit, bool> CancelCommand { get; }
    #endregion Commands

    #region Properties
    [Reactive] public string? Title { get; set; }
    [Reactive] public string? Message { get; set; }
    public string? Text { get; set; }
    #endregion Properties

    #region Result
    private readonly TaskCompletionSource<string?> _result =
        new(TaskCreationOptions.RunContinuationsAsynchronously);
    public Task<string?> Result => _result.Task;
    #endregion Result

    public MessageDialogBoxViewModel(string? message)
    {
        Message = message;

        ConfirmCommand = ReactiveCommand.Create(() => _result.TrySetResult(Text));
        CancelCommand = ReactiveCommand.Create(() => _result.TrySetResult(""));
    }

    public MessageDialogBoxViewModel(string? message, string? title) : this(message)
    {
        Title = title;
    }

}
