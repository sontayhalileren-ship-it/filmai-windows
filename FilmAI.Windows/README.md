# Film Keşif — Windows Masaüstü Uygulaması

Premium, koyu temalı bir film keşif/watchlist uygulaması iskeleti. **AI/LLM entegrasyonu yoktur** — öneriler ve "benzer filmler" tamamen kural tabanlı (tür eşleşmesi, minimum puan, kullanıcı tercihleri) hesaplanır.

## Teknoloji

- .NET 8, WinUI 3 (Windows App SDK)
- MVVM: `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`)
- DI: `Microsoft.Extensions.DependencyInjection`
- Yerel veri: `Microsoft.Data.Sqlite` (henüz bağlanmadı — şu an `MockMovieService` sahte veriyle çalışıyor)

## Neden AI yok?

Orijinal proje dokümanlarında Claude/Gemini tabanlı bir "AI film danışmanı" planlanmıştı. Karar değişti: uygulama **filtre ve kural tabanlı** çalışacak. Bu yüzden:

- `IMovieService` içinde `AskAssistant`, `GetAiRecommendations` gibi hiçbir metot yok.
- "Günün Önerisi" → `GetTonightsPickAsync`: sevilen türler + min puan filtresiyle en yüksek puanlı, izlenmemiş filmi seçer.
- "Benzer Filmler" → `GetSimilarMoviesAsync`: ortak tür sayısına göre sıralar.
- Ayarlar sayfasında hiçbir AI sağlayıcı API anahtarı alanı yok.
- `.csproj` içinde hiçbir AI/LLM SDK paketi referanslanmıyor.

## Klasör Yapısı

```
FilmAI.Windows/
├── App.xaml(.cs)              # DI container kurulumu
├── MainWindow.xaml(.cs)       # NavigationView: Home, Discover, Watchlist, Settings
├── Models/                    # Movie, CastMember, CrewMember, UserPreference, MovieSearchFilter
├── ViewModels/                # MainViewModel, HomeViewModel, DiscoverViewModel,
│                               MovieDetailViewModel, WatchlistViewModel
├── Views/                     # HomePage, DiscoverPage, MovieDetailPage, WatchlistPage, SettingsPage
├── Controls/                  # MovieCard (poster + puan rozeti + hidden-gem etiketi)
├── Services/                  # IMovieService / MockMovieService, INavigationService
├── Styles/                    # Colors.xaml (#121212 / #E50914 / #F5C518), Typography.xaml, ControlStyles.xaml
└── app.manifest
```

## Nasıl Çalıştırılır

Bu proje **Windows'a özgüdür** (WinUI 3) ve bu sohbetin çalıştığı Linux ortamında derlenemez/çalıştırılamaz — kaynak kodu burada üretildi, derleme ve test Windows tarafında yapılmalı.

1. Visual Studio 2022 (17.9+) kurun, **".NET Desktop Development"** ve **"Windows App SDK C# Templates"** iş yüklerini ekleyin.
2. Bu klasörü açın, `dotnet restore` çalıştırın (veya VS otomatik yapar).
3. `x64` platformunu seçip **F5** ile çalıştırın.
4. `MockMovieService` içindeki örnek 8 filmle uygulama doğrudan gezilebilir durumda gelir (Home, Discover filtreleri, film detayı, watchlist ekle/çıkar, puanlama).

## Sıradaki Adımlar (öneri)

- `MockMovieService`'i gerçek TMDB API çağrılarıyla değiştiren bir `TmdbMovieService` eklemek.
- SQLite ile watchlist/puanların kalıcı hale getirilmesi.
- `DiscoverPage`'deki ComboBox (sıralama) için gerçek `SelectionChanged` bağlaması.
