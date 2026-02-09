# Zadanie rekrutacyjne

## Cel zadania

Sprawdzenie umiejętności:
- Projektowania aplikacji zgodnie z **Onion/Clean Architecture**
- Tworzenia UI w **Blazor Server**
- Integracji z AI przy użyciu **Microsoft Agent Framework**
- Pracy z **Claude Code**

---

## Scenariusz biznesowy

### 🐾 Sklep internetowy "PetWorld"

**PetWorld** to sklep internetowy oferujący produkty dla zwierząt domowych. Klienci mogą zadawać pytania o produkty poprzez chat, a system AI pomaga im znaleźć odpowiednie produkty i udziela porad.

### Katalog produktów (do wpisania w prompt)

| Nazwa produktu | Kategoria | Cena | Opis |
|----------------|-----------|------|------|
| Royal Canin Adult Dog 15kg | Karma dla psów | 289 zł | Premium karma dla dorosłych psów średnich ras |
| Whiskas Adult Kurczak 7kg | Karma dla kotów | 129 zł | Sucha karma dla dorosłych kotów z kurczakiem |
| Tetra AquaSafe 500ml | Akwarystyka | 45 zł | Uzdatniacz wody do akwarium, neutralizuje chlor |
| Trixie Drapak XL 150cm | Akcesoria dla kotów | 399 zł | Wysoki drapak z platformami i domkiem |
| Kong Classic Large | Zabawki dla psów | 69 zł | Wytrzymała zabawka do napełniania smakołykami |
| Ferplast Klatka dla chomika | Gryzonie | 189 zł | Klatka 60x40cm z wyposażeniem |
| Flexi Smycz automatyczna 8m | Akcesoria dla psów | 119 zł | Smycz zwijana dla psów do 50kg |
| Brit Premium Kitten 8kg | Karma dla kotów | 159 zł | Karma dla kociąt do 12 miesiąca życia |
| JBL ProFlora CO2 Set | Akwarystyka | 549 zł | Kompletny zestaw CO2 dla roślin akwariowych |
| Vitapol Siano dla królików 1kg | Gryzonie | 25 zł | Naturalne siano łąkowe, podstawa diety |

---

## Wymagania

### Funkcjonalne

**Strona 1 - Chat z klientem**
- Pole tekstowe do wpisania pytania klienta
- Przycisk "Wyślij"
- Wyświetlenie odpowiedzi + liczba iteracji Writer-Critic

**Strona 2 - Historia**
- DataGrid z listą pytań i odpowiedzi
- Kolumny: Data, Pytanie, Odpowiedź, Liczba iteracji

**System AI - Writer-Critic**
- **Writer Agent** - generuje odpowiedź dla klienta, rekomenduje produkty
- **Critic Agent** - ocenia odpowiedź, zwraca `approved: true/false` + feedback
- Maksymalnie **3 iteracje**
- Wykorzystaj: https://github.com/microsoft/agent-framework

**Baza danych (MySQL)**


### Techniczne

- **Architektura:** Onion/Clean Architecture (wymagane)
- **UI:** Blazor Server
- **Baza:** MySQL
- **AI:** Microsoft Agent Framework
- **Uruchomienie:** `docker compose up`

---

## Uruchomienie

Aplikacja **MUSI** uruchamiać się jednym poleceniem:

```bash
docker compose up
```

Po uruchomieniu:
- Aplikacja: http://localhost:5000

Klucz API (OpenAI/Azure) konfigurowalny przez zmienną środowiskową lub `appsettings.json`.

---

## Kryteria oceny

- Poprawna implementacja Onion/Clean Architecture
- Działający Writer-Critic workflow
- Aplikacja uruchamia się przez `docker compose up`
- Czytelny kod

---

## Dostawa rozwiązania

### Repozytorium Git (PUBLICZNE)

Udostępnij link do **publicznego** repozytorium GitHub/GitLab zawierającego:
- Kod źródłowy
- `docker-compose.yml`
- `README.md` z instrukcją

### Czas realizacji

**Tak szybko jak to możliwe.**

### Narzędzia

Zadanie wykonaj przy użyciu **Claude Code**.

---

## Rozmowa techniczna

Po dostarczeniu rozwiązania przeprowadzimy rozmowę techniczną, podczas której:

1. Wyjaśnisz architekturę i przepływ zależności między warstwami
2. Opowiesz jak działa komunikacja między agentami
3. Wykonasz drobne modyfikacje kodu na żywo (bez AI)

**Musisz być w stanie wyjaśnić każdą linię kodu w projekcie.**

---
**Powodzenia! 🐾**
