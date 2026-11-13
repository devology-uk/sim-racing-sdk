using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimRacingSdk.DataManager.Shared.Controls;

// Selects a TextBox's whole value when it gets focus, by tab or by click, so typing replaces it.
public static class SelectAllOnFocus
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(SelectAllOnFocus),
        new PropertyMetadata(false, OnIsEnabledChanged));

    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }

    private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args)
    {
        ((TextBox)sender).SelectAll();
    }

    private static void OnIsEnabledChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
    {
        if(element is not TextBox textBox)
        {
            return;
        }

        if((bool)args.NewValue)
        {
            textBox.GotKeyboardFocus += OnGotKeyboardFocus;
            textBox.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }
        else
        {
            textBox.GotKeyboardFocus -= OnGotKeyboardFocus;
            textBox.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }
    }

    // Without this, the click that gives focus also places the caret and clears the selection.
    private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
    {
        var textBox = (TextBox)sender;
        if(textBox.IsKeyboardFocusWithin)
        {
            return;
        }

        textBox.Focus();
        args.Handled = true;
    }
}
