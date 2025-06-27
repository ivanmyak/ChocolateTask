# ChocolateTask / ChocolateAPI

GraphQL API для управления задачами с JWT-аутентификацией и ролевой авторизацией.

## 1. Требования
- .NET 8.0 SDK  
- Docker & Docker Compose (опционально)  
- PostgreSQL (локально или в контейнере)

## 2. Клонирование и локальная разработка

```bash
git clone <repo-url>
cd ChocolateTask/ChocolateAPI

# 1) Скопировать шаблон .env
cp .env.example .env

# 2) Отредактировать .env:
#   ASPNETCORE_ENVIRONMENT=Development
#   ChocolateConnectionString=<твоя строка>
#   JwtSettings__Issuer=<Issuer>
#   JwtSettings__Audience=<Audience>
#   JwtSettings__SecretKey=<секрет, ≥32 символов>

# 3) Запуск из VS 2022 — выбрать профиль "ChocolateAPI"
#или
dotnet run
