# Testing Notes

Show the reminder immediately:

```powershell
.\scripts\test-alert.ps1
```

Use a short timer interval:

```powershell
$env:REMANDME_INTERVAL_SECONDS='10'
dotnet run -- --no-startup
```

When testing is finished, right-click the penguin icon in the system tray and choose Exit.
