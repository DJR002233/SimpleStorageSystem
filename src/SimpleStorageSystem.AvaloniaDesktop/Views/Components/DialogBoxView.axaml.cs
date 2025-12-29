using System;
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

    public DialogBoxView(string title = "", string message = "") : this()
    {
        this.Title = title;
        MessageTextBlock.Text = message;
        OkButton.IsVisible = true;
        SystemDecorations = SystemDecorations.Full;
    }

    public DialogBoxView(
        string title, string message,
        DialogBoxButtons buttons
    ) : this()
    {
        this.Title = title;
        MessageTextBlock.Text = message;

        if (buttons == DialogBoxButtons.Ok)
            OkButton.IsVisible = true;
        else if (buttons == DialogBoxButtons.ConfirmCancel)
        {
            ConfirmButton.IsVisible = true;
            CancelButton.IsVisible = true;
        }
        else throw new Exception("DialogBoxButtons is null");

        this.SystemDecorations = SystemDecorations.Full;
    }

    public DialogBoxView(
        string title, string message,
        SystemDecorations buttons
    ) : this()
    {
        this.Title = title;
        MessageTextBlock.Text = message;
        this.SystemDecorations = buttons;

        OkButton.IsVisible = true;
    }

    public DialogBoxView(
        string title, string message,
        DialogBoxButtons buttons, SystemDecorations decorations
    ) : this()
    {
        this.Title = title;
        MessageTextBlock.Text = message;

        if (buttons == DialogBoxButtons.Ok)
            OkButton.IsVisible = true;
        else if (buttons == DialogBoxButtons.ConfirmCancel)
        {
            ConfirmButton.IsVisible = true;
            CancelButton.IsVisible = true;
        }
        else throw new Exception("DialogBoxButtons is null");

        this.SystemDecorations = decorations;
    }

    public DialogBoxView(
        string title, string message,
        SystemDecorations decorations, DialogBoxButtons buttons
    ) : this()
    {
        this.Title = title;
        MessageTextBlock.Text = message;

        if (buttons == DialogBoxButtons.Ok)
            OkButton.IsVisible = true;
        else if (buttons == DialogBoxButtons.ConfirmCancel)
        {
            ConfirmButton.IsVisible = true;
            CancelButton.IsVisible = true;
        }
        else throw new Exception("DialogBoxButtons is null");

        this.SystemDecorations = decorations;
    }

    private void Ok(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        if (InputTextBox.IsVisible)
        {
            Close(InputTextBox.Text);
            return;
        }
        Close(true);
    }

    private void Close(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }

}
