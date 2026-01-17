using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

public class ConfirmationDialogBoxViewModel : ReactiveObject
{
    #region Commands
    public ReactiveCommand<Unit, bool> ConfirmCommand { get; }
    public ReactiveCommand<Unit, bool> CancelCommand { get; }
    #endregion Commands

    #region Properties
    [Reactive] public string? Title { get; set; }
    [Reactive] public string? Message { get; set; }
    [Reactive] public bool OkButtonIsVisible { get; set; } = false;
    [Reactive] public bool ConfirmCancelButtonIsVisible { get; set; } = false;
    #endregion Properties

    #region Result
    private readonly TaskCompletionSource<bool> _result =
        new(TaskCreationOptions.RunContinuationsAsynchronously);
    public Task<bool> Result => _result.Task;
    #endregion Result

    public ConfirmationDialogBoxViewModel(string? message, bool OkOnly = true)
    {
        if (OkOnly) OkButtonIsVisible = true;
        else ConfirmCancelButtonIsVisible = true;

        Message = message;

        ConfirmCommand = ReactiveCommand.Create(() => _result.TrySetResult(true));
        CancelCommand = ReactiveCommand.Create(() => _result.TrySetResult(false));
    }

    public ConfirmationDialogBoxViewModel(string? message, string? title, bool OkOnly = false) : this(message, OkOnly)
    {
        Title = title;
    }

}
