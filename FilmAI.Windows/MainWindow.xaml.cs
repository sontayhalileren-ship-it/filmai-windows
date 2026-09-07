using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FilmAI.Windows.Services;
using FilmAI.Windows.Views;

namespace FilmAI.Windows;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Title = "Film Keşif";

        // Mica arka plan (Windows 11). Windows 10'da SystemBackdrop null kalır,
        // AppBackgroundBrush fallback rengi kullanılır (bkz. Styles/Colors.xaml).
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            SystemBackdrop = new MicaBackdrop();
        }

        var navService = (NavigationService)App.Services.GetService(typeof(INavigationService))!;
        navService.Initialize(ContentFrame);

        ContentFrame.Navigate(typeof(HomePage));
        RootNavigationView.SelectedItem = RootNavigationView.MenuItems[0];
    }

    private void RootNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
            return;
        }

        var tag = (args.InvokedItemContainer as NavigationViewItem)?.Tag as string;
        Type? pageType = tag switch
        {
            "Home" => typeof(HomePage),
            "Discover" => typeof(DiscoverPage),
            "Watchlist" => typeof(WatchlistPage),
            _ => null,
        };

        if (pageType is not null && ContentFrame.CurrentSourcePageType != pageType)
            ContentFrame.Navigate(pageType);
    }

    private void RootNavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (ContentFrame.CanGoBack)
            ContentFrame.GoBack();
    }
}
