using Avalonia.Controls;
using Avalonia.Interactivity;
using SimpleStorageSystem.AvaloniaDesktop.Enums;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Components;

public partial class DialogBoxView : Window
{
    public DialogBoxView()
    {
        InitializeComponent();
    }

    private DialogBoxView(DialogBoxMode buttons, SystemDecorations decorations) : this()
    {
        this.SystemDecorations = decorations;

        if (buttons == DialogBoxMode.OkOnly)
            OkButton.IsVisible = true;
        else if (buttons == DialogBoxMode.ConfirmCancel)
        {
            ConfirmButton.IsVisible = true;
            CancelButton.IsVisible = true;
        }else if (buttons == DialogBoxMode.InputText)
        {
            InputTextBox.IsVisible = true;
            ConfirmButton.IsVisible = true;
            CancelButton.IsVisible = true;
        }
    }

    public DialogBoxView(
        string title = "", string message = ""
    ) : this(DialogBoxMode.OkOnly, SystemDecorations.Full)
    {
        this.Title = title;
        MessageTextBlock.Text = message;
    }

    public DialogBoxView(
        string title, string message,
        DialogBoxMode buttons
    ) : this(buttons, SystemDecorations.Full)
    {
        this.Title = title;
        MessageTextBlock.Text = message;
    }

    public DialogBoxView(
        string title, string message,
        SystemDecorations decorations
    ) : this(DialogBoxMode.OkOnly, decorations)
    {
        this.Title = title;
        MessageTextBlock.Text = message;
    }

    public DialogBoxView(
        string title, string message,
        DialogBoxMode buttons, SystemDecorations decorations
    ) : this(buttons, decorations)
    {
        this.Title = title;
        MessageTextBlock.Text = message;
    }

    public DialogBoxView(
        string title, string message,
        SystemDecorations decorations, DialogBoxMode buttons
    ) : this(buttons, decorations)
    {
        this.Title = title;
        MessageTextBlock.Text = message;
    }

    private void Ok(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        if (InputTextBox.IsVisible) Close(InputTextBox.Text);
        else if (!InputTextBox.IsVisible) Close(true);
        else Close();
        
    }

    private void Close(object? sender, RoutedEventArgs e)
    {
        if (InputTextBox.IsVisible) Close(null);
        else if (!InputTextBox.IsVisible) Close(true);
        else Close();
    }

}
