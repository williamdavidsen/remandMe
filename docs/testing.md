# Test Notlari

Hemen uyari ekranini gormek icin:

```powershell
.\scripts\test-alert.ps1
```

Kisa sureli zamanlayici testi icin:

```powershell
$env:REMANDME_INTERVAL_SECONDS='10'
dotnet run -- --no-startup
```

Test bittiginde sistem tepsisindeki penguen ikonuna sag tiklayip cikis secilebilir.
