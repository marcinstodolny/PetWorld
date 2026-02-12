# PetWorld

Aplikacja webowa **Blazor Server** dla sklepu zoologicznego PetWorld, zrealizowana w podejściu **Onion/Clean Architecture**.  
System udostępnia chat wspierany przez AI (workflow Writer-Critic) oraz historię rozmów zapisaną w bazie MySQL.

## Zakres funkcjonalny

### 1) Chat z klientem
- Formularz do wpisania pytania.
- Przycisk **Wyślij**.
- Odpowiedź AI wraz z liczbą iteracji Writer-Critic (max 3).

### 2) Historia rozmów
- Widok tabelaryczny (QuickGrid) z zapisanymi wiadomościami.
- Kolumny: **Data**, **Pytanie**, **Odpowiedź**, **Liczba iteracji**.

## Architektura projektu

Rozwiązanie podzielone jest na warstwy:
- `PetWorld.Domain` – encje, value objecty, reguły domenowe.
- `PetWorld.Application` – use case’y i kontrakty (repozytoria, AI).
- `PetWorld.Infrastructure` – EF Core, MySQL, migracje, implementacje repozytoriów i Writer-Critic.
- `PetWorld.Web` – interfejs użytkownika (Blazor Server).

## Stos technologiczny

- **.NET 10 / C#**
- **Blazor Server**
- **MySQL 8**
- **Entity Framework Core**
- **Microsoft Agent Framework**
- **Docker Compose**

## Uruchomienie (Docker Compose)

Zgodnie z wymaganiami zadania projekt jest opisany pod uruchamianie przez Docker Compose.

### 1) Przygotuj konfigurację (wybierz jedną opcję)

**Opcja A: `.env` (zalecana przy Docker Compose)**

Skopiuj przykład zmiennych:

```bash
cp .env.example .env
```

Następnie uzupełnij w `.env` co najmniej:
- `OPENAI_API_KEY`

Przykład zawartości `.env`:

```env
OPENAI_API_KEY=your_openai_api_key_here
MYSQL_ROOT_PASSWORD=admin
MYSQL_DATABASE=petworld
MYSQL_USER=petworld
MYSQL_PASSWORD=petworld
```

**Opcja B: `appsettings.json` (tylko klucz AI)**

Możesz ustawić klucz także w:
- `src/PetWorld.Web/appsettings.json` → `AgentFramework:OpenAiApiKey`

> Priorytet: jeśli ustawisz oba, **użyty zostanie `appsettings.json`** (`AgentFramework:OpenAiApiKey`), a dopiero gdy jest pusty – `OPENAI_API_KEY` z .env.

### 2) Uruchom aplikację

```bash
docker compose up --build
```

Po uruchomieniu:
- Aplikacja: `http://localhost:5000`
- MySQL: `localhost:3307`

## Przydatne adresy w aplikacji

- `/chat` – chat z klientem
- `/chat-history` – historia zapytań

## Testy

```bash
dotnet test PetWorld.slnx
```