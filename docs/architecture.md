# Mimari

RemandMe tek surecli bir WinForms uygulamasidir.

- `Program.cs`: uygulama girisi, sistem tepsisi, zamanlayici, Windows baslangic kaydi ve uyku modu donusu.
- `AlertForm.cs`: penguenli uyari ekraninin cizimi ve buton yerlesimi.
- `PenguinIcon.cs`: sistem tepsisi icin dosyasiz uretilen penguen ikonu.

Uygulama varsayilan olarak 20 dakikada bir uyari gosterir. Testlerde `REMANDME_INTERVAL_SECONDS` ortam degiskeni ile sure kisaltilabilir.
