RemandMe
========

Windows 11 icin cok kucuk bir ayaga kalk hatirlaticisi.

Ne yapar?

- Bilgisayar acilinca otomatik baslar.
- Uyku modundan donunce 20 dakikalik sayaci yeniden baslatir.
- Her 20 dakikada bir ekranin yaklasik %70 x %60'ini kaplayan penguenli uyari gosterir.
- Sistem tepsisinde sessiz calisir.

Calistirma

```powershell
dotnet run
```

Test icin hemen uyari gostermek:

```powershell
dotnet run -- --show-now
```

Testte 20 dakika beklememek icin:

```powershell
$env:REMANDME_INTERVAL_SECONDS='10'
dotnet run
```

Tek exe uretmek:

```powershell
dotnet publish -c Release
```

Exe su klasore gelir:

```text
bin\Release\net8.0-windows\win-x64\publish\RemandMe.exe
```

Baslangictan kaldirmak:

```powershell
dotnet run -- --uninstall-startup
```
