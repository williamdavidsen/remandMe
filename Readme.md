RemandMe
========

RemandMe is a small Windows 11 stand-up reminder. It runs quietly in the system tray, starts with Windows by default, and shows a penguin reminder every 20 minutes so you can move, stretch, and drink water.

## What it does

- Starts automatically when Windows starts.
- Restarts the 20-minute timer after the computer wakes from sleep.
- Shows a large penguin reminder every 20 minutes.
- Runs from the system tray with quick actions.

## Run

```powershell
dotnet run
```

Show the reminder immediately:

```powershell
dotnet run -- --show-now
```

Use a shorter interval for testing:

```powershell
$env:REMANDME_INTERVAL_SECONDS='10'
dotnet run
```

## Build the exe

```powershell
dotnet publish -c Release
```

The exe is created here:

```text
bin\Release\net8.0-windows\win-x64\publish\RemandMe.exe
```

Create a ready-to-share copy:

```powershell
.\scripts\publish.ps1
```

This creates `RemandMe.exe`, `Test Now.cmd`, and `Remove From Startup.cmd` in `dist\RemandMe`.

Remove it from Windows startup:

```powershell
dotnet run -- --uninstall-startup
```
