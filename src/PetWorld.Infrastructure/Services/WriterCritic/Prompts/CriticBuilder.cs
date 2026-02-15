using System.Text;

namespace PetWorld.Infrastructure.Services.WriterCritic.Prompts;

public sealed class CriticBuilder : ICriticBuilder
{
    public string BuildInstructions(int feedbackMaxLength)
    {
        return $$"""
                  Jesteś krytykiem jakości odpowiedzi AI w sklepie PetWorld.
                  
                  Sprawdź odpowiedź Writer’a według reguł MAŁEGO KATALOGU.
                  
                  KONTRAKT FORMATU OD WRITERA:
                  - Linia 1: dokładnie 1 zdanie wstępu.
                  - Kolejne linie: 0–3 rekomendacje, każda osobno, bez prefiksów listy, format: Nazwa - cena - krótkie uzasadnienie.
                  - Opcjonalnie ostatnia linia: "Pytanie: ...".
                  
                  WARUNKI ODRZUCENIA (approved=false):
                  - Jakikolwiek produkt spoza katalogu lub nazwa nie jest identyczna z katalogiem.
                  - Writer dopowiada cechy produktu, których nie ma w katalogu.
                  - Writer używa markdown/JSON/potrójnych backticków ``` w odpowiedzi.
                  - Brak dokładnie 1 zdania wstępu w pierwszej linii.
                  - Rekomendacje nie są w osobnych liniach lub jest ich więcej niż 3.
                  - Writer używa listy punktowanej/numerycznej (np. '-', '*', '1.').
                  - Linia "Pytanie: ..." występuje, ale nie jest ostatnia.
                  - Writer podał „najbliższe dostępne”, ale nie wyjaśnił krótko dlaczego (1 krótka przyczyna).
                  - Odpowiedź jest rażąco nie na temat.
                  
                  LICZBA PRODUKTÓW:
                  - 2–3 to cel, ale dopuszczalne jest 0–3:
                  - 0: gdy brak sensownego dopasowania lub pytanie niezrozumiałe.
                  - 1: gdy w katalogu jest tylko jeden sensownie pasujący produkt.
                  - 2–3: gdy są sensowne dopasowania.
                  
                  Zwróć WYŁĄCZNIE jeden obiekt JSON (bez markdown, bez komentarzy, bez ```):
                  {"approved": true|false, "feedback": "jedna najważniejsza wskazówka poprawy po polsku (max {{feedbackMaxLength}} znaków)"}
                  
                  """;
    }

    public string BuildPrompt(string question, string answer, string catalog)
    {
        var builder = new StringBuilder();
        builder.AppendLine("OCEŃ odpowiedź Writer’a. Zwróć TYLKO JSON.");
        builder.AppendLine("Mały katalog: 0–3 rekomendacje są dopuszczalne (cel 2–3).");
        builder.AppendLine();

        builder.AppendLine("=== PYTANIE KLIENTA (DANE) ===");
        builder.AppendLine(question);
        builder.AppendLine();

        builder.AppendLine("=== ODPOWIEDŹ WRITER’A (DANE) ===");
        builder.AppendLine(answer);
        builder.AppendLine();

        builder.AppendLine("=== KATALOG (DANE / WERYFIKACJA) ===");
        builder.AppendLine(catalog);
        builder.AppendLine();

        builder.AppendLine("Zwróć WYŁĄCZNIE jeden JSON: {\"approved\":true|false,\"feedback\":\"...\"}");

        return builder.ToString();
    }
}
