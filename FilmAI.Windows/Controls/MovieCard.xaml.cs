using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using FilmAI.Windows.Models;

namespace FilmAI.Windows.Controls;

public sealed partial class MovieCard : UserControl
{
    public static readonly DependencyProperty MovieProperty =
        DependencyProperty.Register(nameof(Movie), typeof(Movie), typeof(MovieCard), new PropertyMetadata(null));

    public Movie Movie
    {
        get => (Movie)GetValue(MovieProperty);
        set => SetValue(MovieProperty, value);
    }

    public event RoutedEventHandler? Clicked;

    public MovieCard()
    {
        InitializeComponent();
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        Clicked?.Invoke(this, new RoutedEventArgs());
    }
}
