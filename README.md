# PetWorld

Aplikacja webowa **Blazor Server** dla sklepu zoologicznego PetWorld, zrealizowana w podejściu **Onion/Clean Architecture**.  
System udostępnia chat wspierany przez AI (workflow Writer-Critic) oraz historię rozmów zapisaną w bazie MySQL.

## Zakres funkcjonalny

### 1) Chat z klientem
- Formularz do wpisania pytania.
- Przycisk **Wyślij**.
- Możliwość wyboru modelu z listy rozwijanej (opcjonalnie).
- Lista modeli pochodzi z `AgentFramework:AvailableModels`, a domyślny wybór z `AgentFramework:DefaultModel`.
- Odpowiedź AI wraz z liczbą iteracji Writer–Critic (max 3).

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
MYSQL_HOST_PORT=3307
```

**Opcja B: `appsettings.json` **

Możesz ustawić klucz także w:
- `src/PetWorld.Web/appsettings.json` → `AgentFramework:OpenAiApiKey`

> Priorytet: najpierw używany jest `AgentFramework:OpenAiApiKey`, a jeśli jest pusty, to `OPENAI_API_KEY`.

### Wybór modelu (opcjonalnie)

Na `/chat` dostępna jest lista rozwijana modelu.

Modele konfiguruje się w appsettings.json przez:
- `AgentFramework:DefaultModel`
- `AgentFramework:AvailableModels`

### 2) Uruchom aplikację

```bash
docker compose up
```

Jeśli zmieniłeś kod i chcesz przebudować obraz:

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

Aktualny stan testów:
- ✅ Unit testy (logika domenowa i bazowe elementy aplikacji).
- 🟡 Projekt testów integracyjnych istnieje, ale jest na etapie przygotowania i będzie rozwijany w kolejnych iteracjach.

## Status rozwiązania

Aktualna wersja projektu dostarcza działające **MVP** zgodne z głównymi wymaganiami zadania:
- architektura Onion/Clean,
- interfejs Blazor Server,
- workflow Writer-Critic,
- uruchomienie przez `docker compose up`.

### Aktualne ograniczenia

- Obecnie zaimplementowana i zweryfikowana jest integracja z **OpenAI**.
- Wsparcie **Azure OpenAI** jest zaplanowane jako kolejny krok.

## Plany dalszego rozwoju

1. **Testy integracyjne (Testcontainers + MySQL)**
   - uruchamianie bazy MySQL w kontenerze na czas testów,
   - testowanie kluczowych scenariuszy end-to-end dla warstwy Infrastructure/Application,
   - walidacja zapisu i odczytu historii rozmów.

2. **Wsparcie Azure OpenAI**
   - dodanie alternatywnej konfiguracji providera AI,
   - możliwość przełączania OpenAI / Azure OpenAI przez konfigurację środowiskową,
   - utrzymanie spójnego kontraktu w warstwie Application.

3. **Rozszerzenie pokrycia testami**
   - smoke test przepływu Writer-Critic,
   - testy negatywne dla błędnej konfiguracji kluczy/API.
