# FilmAI Windows — GitHub Actions ile .exe Derleme

Bu proje bir **WinUI 3 (.NET 8) Windows uygulaması**. WinUI3/WPF projeleri Windows dışında derlenemediği için, GitHub'ın ücretsiz Windows sunucularını (GitHub Actions) kullanarak otomatik derleme kurdum.

## Adımlar

1. **GitHub'da yeni bir repo oluşturun** (public ya da private, fark etmez).
   https://github.com/new

2. **Bu klasörün TÜM içeriğini** (hem `FilmAI.Windows/` klasörünü hem de `.github/` klasörünü) reponun **kök dizinine** yükleyin.

   Terminal ile (klasörün içindeyken):
   ```bash
   git init
   git add .
   git commit -m "İlk yükleme"
   git branch -M main
   git remote add origin https://github.com/KULLANICI_ADIN/REPO_ADIN.git
   git push -u origin main
   ```

   Ya da GitHub web arayüzünden "Upload files" ile sürükle-bırak yapabilirsiniz (gizli `.github` klasörünü yüklediğinizden emin olun — bazı dosya yöneticileri nokta ile başlayan klasörleri gizler).

3. Push işleminden sonra GitHub otomatik olarak derlemeyi başlatır. Reponuzda **"Actions"** sekmesine gidin, çalışan (veya biten) iş akışını (workflow) göreceksiniz — adı **"Build Windows EXE"**.

4. Derleme bitince (genelde 3-6 dakika sürer) o çalışmanın sayfasına tıklayın, en altta **"Artifacts"** bölümünde **`FilmAI-Windows-x64`** adlı bir zip dosyası göreceksiniz. Onu indirin.

5. İndirdiğiniz zip'i açtığınızda içinde **`FilmAI.Windows.exe`** dosyasını bulacaksınız — çift tıklayıp Windows'ta doğrudan çalıştırabilirsiniz (kurulum/.NET indirmesi gerekmez, self-contained'dır).

## Elle tekrar tetiklemek isterseniz
Actions sekmesinde soldan "Build Windows EXE" workflow'unu seçip sağ üstten **"Run workflow"** butonuna basmanız yeterli.

## Notlar
- Workflow dosyası: `.github/workflows/build-windows.yml`
- Varsayılan olarak sadece **x64** için derler. `x86` veya `arm64` için de derleme istiyorsanız, workflow dosyasındaki `matrix.arch` listesine ekleyebilirsiniz.
- Uygulama WinUI3 kullandığı için ilk açılışta bilgisayarınızda **Windows App Runtime** kurulu olması gerekebilir; self-contained publish (`WindowsAppSDKSelfContained=true`) bunu genelde otomatik hallediyor ama sorun yaşarsanız Microsoft'un Windows App SDK Runtime kurulumunu indirmeniz gerekebilir.
