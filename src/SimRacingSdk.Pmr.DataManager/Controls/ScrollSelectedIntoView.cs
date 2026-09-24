using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SimRacingSdk.Pmr.DataManager.Controls;

// Keeps a DataGrid's selected row on screen - needed when a restored session selects a row far
// down a long list, or when a page is re-shown after its selection changed elsewhere.
public static class ScrollSelectedIntoView
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(ScrollSelectedIntoView),
        new PropertyMetadata(false, OnIsEnabledChanged));

    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }

    private static void OnIsEnabledChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
    {
        if(element is not DataGrid dataGrid)
        {
            return;
        }

        if((bool)args.NewValue)
        {
            dataGrid.Loaded += OnLoaded;
            dataGrid.SelectionChanged += OnSelectionChanged;
        }
        else
        {
            dataGrid.Loaded -= OnLoaded;
            dataGrid.SelectionChanged -= OnSelectionChanged;
        }
    }

    private static void OnLoaded(object sender, RoutedEventArgs args)
    {
        ScrollToSelectedItem((DataGrid)sender);
    }

    private static void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        ScrollToSelectedItem((DataGrid)sender);
    }

    // Deferred until after layout, otherwise rows that haven't been generated yet can't be scrolled to.
    private static void ScrollToSelectedItem(DataGrid dataGrid)
    {
        dataGrid.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, () =>
        {
            if(dataGrid.SelectedItem is not null)
            {
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
            }
        });
    }
}
