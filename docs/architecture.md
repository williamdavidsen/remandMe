# Architecture

RemandMe is a single-process WinForms application.

- `Program.cs`: application entry point, tray icon, timer, Windows startup registration, and wake-from-sleep handling.
- `AlertForm.cs`: drawing and button layout for the penguin reminder screen.
- `PenguinIcon.cs`: generated in-memory penguin icon for the system tray.

The app shows a reminder every 20 minutes by default. Tests can shorten the interval with the `REMANDME_INTERVAL_SECONDS` environment variable.
