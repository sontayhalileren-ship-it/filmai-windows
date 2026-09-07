using CommunityToolkit.Mvvm.ComponentModel;

namespace FilmAI.Windows.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string appTitle = "Film Keşif";
}
