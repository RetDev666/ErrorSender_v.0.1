# ErrorSender_v.0.1

Сервіс для відправки повідомлень про помилки через Telegram бота.

## Опис

ErrorSender - це API сервіс, який дозволяє додаткам надсилати повідомлення про помилки через Telegram бота. Це дозволяє швидко отримувати інформацію про помилки у додатках без необхідності постійно перевіряти логи.

## Функціональні можливості

- Відправка повідомлень про помилки в Telegram
- API ключі для безпечного доступу
- JWT автентифікація для захищених ендпоінтів
- Детальна інформація про помилки (стек виклику, додаткова інформація, тощо)
- Зручний формат повідомлень у Telegram

## Технології

- .NET 8.0
- ASP.NET Core Web API
- MediatR для реалізації CQRS
- FluentValidation для валідації
- Swagger для документації API
- JWT для автентифікації
- Telegram.Bot для інтеграції з Telegram API

## Встановлення та налаштування

### Вимоги

- .NET 8.0 SDK
- Telegram бот (створений через BotFather)

### Кроки встановлення

1. Клонувати репозиторій
2. Налаштувати файл appsettings.json
3. Запустити проект

```bash
# Клонування репозиторію
git clone https://github.com/yourusername/ErrorSender_v.0.1.git
cd ErrorSender_v.0.1

# Збірка проекту
dotnet build

# Запуск проекту
dotnet run --project ErrSendWebApi
```

## Налаштування Telegram бота

1. Створіть нового бота через [@BotFather](https://t.me/botfather) в Telegram
2. Отримайте токен бота
3. Створіть групу/канал і додайте бота як адміністратора
4. Отримайте chat_id групи/каналу (можна через API або спеціальні боти)
5. Додайте ці дані в `appsettings.json`

## Конфігурація (appsettings.json)

```json
{
  "AllowedHosts": "*",
  "serverUrl": "http://localhost:5001/",
  "Telegram": {
    "Enabled": true,
    "BotToken": "YOUR_BOT_TOKEN",
    "ChatId": "YOUR_CHAT_ID",
    "BotName": "ErrorSenderBot"
  },
  "JWT": {
    "TokenKey": "YOUR_SECRET_KEY",
    "ExpiryInDays": 7
  },
  "ApiKeys": [
    "your-api-key-1",
    "your-api-key-2"
  ],
  "AdminKey": "admin-secure-key",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Опис параметрів конфігурації

- **Telegram:Enabled** - вмикає/вимикає відправку в Telegram
- **Telegram:BotToken** - токен вашого Telegram бота
- **Telegram:ChatId** - ID чату/групи/каналу для відправки повідомлень
- **JWT:TokenKey** - секретний ключ для JWT автентифікації
- **ApiKeys** - список API ключів для доступу до сервісу
- **AdminKey** - ключ для адміністративних функцій

## API ендпоінти

### Відправка помилки

```
POST /api/Telegram/SendError
Header: X-API-KEY: your-api-key-1
Body:
{
  "application": "TestApp",
  "version": "1.0.0",
  "environment": "Development",
  "message": "Текст помилки",
  "stackTrace": "at TestApp.Program.Main() ...",
  "additionalInfo": "Додаткова інформація"
}
```

### Адміністративні ендпоінти

```
POST /api/Admin/GenerateApiKey
Header: X-ADMIN-KEY: admin-secure-key
```

```
POST /api/Admin/GenerateJwtKey
Header: X-ADMIN-KEY: admin-secure-key
```

```
GET /api/Admin/GetSystemStatus
```

## Тестування відправки повідомлень

Використовуйте скрипт `test-error-sender.sh` для тестування відправки повідомлень:

```bash
./test-error-sender.sh
```

Або з використанням curl:

```bash
curl -X POST http://localhost:5001/api/Telegram/SendError \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: your-api-key-1" \
  -d '{
    "application": "TestApp",
    "version": "1.0.0",
    "environment": "Development",
    "message": "Тестова помилка",
    "stackTrace": "at TestApp.Program.Main() in Program.cs:line 12",
    "additionalInfo": "Додаткова інформація"
  }'
```

## Розробка та внесення змін

1. Створіть форк проекту
2. Створіть власну гілку: `git checkout -b my-feature`
3. Внесіть зміни та зробіть коміт: `git commit -m 'Add some feature'`
4. Відправте в свій форк: `git push origin my-feature`
5. Створіть Pull Request

## Структура проекту

- **Domain** - доменні моделі
- **ErrSendApplication** - бізнес-логіка
- **ErrSendPersistensTelegram** - інтеграція з Telegram
- **ErrSendWebApi** - REST API та конфігурація


## Ліцензія

Цей проект розповсюджується під ліцензією MIT. Дивіться файл LICENSE для деталей. 
