using System;

namespace FilmAI.Windows.Services;

public interface INavigationService
{
    void NavigateTo(Type pageType, object? parameter = null);
    bool CanGoBack { get; }
    void GoBack();
}
