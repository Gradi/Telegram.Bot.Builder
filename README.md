# Telegram.Bot.Builder

Telegram.Bot.Builder is a small wrapper around [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) library
which adds ability to use dependency injection and 'controllers like' way to handle incoming updates.

See [Examples](Examples/) directory for sample bots.

## Telegram.Bot.Builder.Webhook

This package adds ability to use webhooks for accepting incoming updates.

It uses ASP.NET Core as web server and it can automatically generate self-signed certificates for
Telegram APIs.

## Third party libraries

This project uses some third party libraries:

- [Bouncy Castle](https://bouncycastle.org/csharp/index.html)'s cryptographic APIs
