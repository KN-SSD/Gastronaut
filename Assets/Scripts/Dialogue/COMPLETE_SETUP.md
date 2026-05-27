# System Questów i Dialogów - Instrukcja Kompletna

## 🎯 Co się zmieniło

Stworzyliliśmy w pełni modularny system questów ze stanami i adaptacyjnymi dialogami.

### Stary flow ❌
```
Dialog → Hardcoded check (if name == "Wanilla") → StartQuests
```

### Nowy flow ✅
```
Quest State Machine:
NotStarted → InProgress → Ready → Completed

Dialogi adaptują się automatycznie na podstawie stanu:
- Wariant 1: "Pomóż mi!" (AlwaysTrue + StartAllQuests)
- Wariant 2: "Jak idą prace?" (QuestInProgress)
- Wariant 3: "Dziękuję!" (AllQuestsCompleted + LevelComplete)
```

## 📌 Najważniejsze Kroki Setup

### 1. Struktura Questów w QuestManager

Dialogi są już gotowe, ale możesz zmienić nazwy questów:

```csharp
public void CreateQuest()
{
    // Format: Quest(nazwa, ilość_do_zboru, quest_giver, quest_receiver)
    quests.Add(new Quest("Red Quest", 3, "Wanilla", "Red Companion"));
    quests.Add(new Quest("Green Quest", 5, "Wanilla", "Green Companion"));
    quests.Add(new Quest("Blue Quest", 3, "Wanilla", "Blue Companion"));
}
```

### 2. Dialogi dla Quest Givera (Wanilla)

W inspektorze na obiekcie Wanilli, edytuj Dialogue Asset:

**Wariant 0 - Początkowy dialog:**
```
Condition: AlwaysTrue
Action: StartAllQuests
Sentences:
  - "Cześć!"
  - "Muszę Ci prosić o pomoc..."
  - "Musisz zbierać przedmioty do 3 zadań"
Quest Name: (dowolny - nie używany w tym wariancie)
```

**Wariant 1 - Po ukończeniu wszystkich:**
```
Condition: AllQuestsCompleted
Action: LevelComplete
Sentences:
  - "Dziękuję! Uratowałeś nas!"
Quest Name: (dowolny)
```

### 3. Dialogi dla Quest Receivera (np. Red Companion)

**Wariant 0 - Quest w trakcie (niezupełny):**
```
Condition: QuestInProgress
Action: None
Sentences:
  - "Jeszcze nie masz wszystkich kryształów..."
  - "Wróć jak zbierzesz 3 sztuki"
Quest Name: "Red Quest"  ← WAŻNE: musi być dokładnie taka sama nazwa!
```

**Wariant 1 - Quest gotów (gracz ma wszystko):**
```
Condition: QuestReady
Action: CompleteQuest
Sentences:
  - "Świetnie! Masz wszystko!"
  - "Dziękuję za pomoc!"
Quest Name: "Red Quest"  ← WAŻNE
```

**Wariant 2 - Quest już ukończony:**
```
Condition: QuestCompleted
Action: None
Sentences:
  - "Dziękuję jeszcze raz!"
Quest Name: "Red Quest"  ← WAŻNE
```

## 🔑 Kluczowy Punkt: Quest Name

**WSZYSTKIE WARUNKI MUSZĄ MIEĆ TAKĄ SAMĄ NAZWĘ QUESTA!**

Na Green Companion:
```
QuestInProgress: "Green Quest"
QuestReady: "Green Quest"
QuestCompleted: "Green Quest"
```

Na Blue Companion:
```
QuestInProgress: "Blue Quest"
QuestReady: "Blue Quest"
QuestCompleted: "Blue Quest"
```

## 📦 Integracja ze zbieraniem przedmiotów

W skrypcie gdzie gracz zbiera przedmioty (np. `PickUpAndHoldItem.cs`):

```csharp
void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Item"))
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            // Dodaj postęp do odpowiedniego questa na podstawie koloru
            if (item.isRed)
                QuestManager.Instance.AddQuestProgress("Red Quest");
            else if (item.isGreen)
                QuestManager.Instance.AddQuestProgress("Green Quest");
            else if (item.isBlue)
                QuestManager.Instance.AddQuestProgress("Blue Quest");
        }
    }
}
```

## 🎮 Jak gracz doświadcza gry

1. Gracz podchodzi do Wanilli
2. Wanilla: "Cześć! Musisz zbierać przedmioty..."
3. UI: "Red Quest: 0/3", "Green Quest: 0/5", "Blue Quest: 0/3"
4. Gracz zbiera przedmioty
5. UI aktualizuje się automatycznie: "Red Quest: 1/3" → "2/3" → "3/3"
6. Gdy Red Quest = 3/3, tekst zmienia kolor na **zielony**: "Red Quest: Ready! Talk to Red Companion"
7. Gracz podchodzi do Red Companion'a
   - Companion: "Świetnie! Masz wszystko! Dziękuję!"
   - UI: "Red Quest: Completed!"
8. Powtarza dla Green i Blue
9. Gdy wszystkie 3 = Completed:
   - Gracz podchodzi do Wanilli
   - Wanilla: "Dziękuję! Uratowałeś nas! Koniec poziomu!"

## 🔧 Warunki Dostępne

| Warunek | Parametr | Opis |
|---------|----------|------|
| `AlwaysTrue` | — | Zawsze spełniony |
| `QuestStarted` | — | Jakiś quest jest aktywny |
| `QuestInProgress` | questName | Konkretny quest w trakcie |
| `QuestReady` | questName | Konkretny quest gotów (gracz ma items) |
| `QuestCompleted` | questName | Konkretny quest ukończony |
| `AllQuestsCompleted` | — | WSZYSTKIE questy ukończone |

## ⚙️ Akcje Dostępne

| Akcja | Parametr | Opis |
|-------|----------|------|
| `None` | — | Brak akcji |
| `StartAllQuests` | — | Uruchom wszystkie questy |
| `CompleteQuest` | questName | Oznacz konkretny quest jako completed |
| `LevelComplete` | — | Koniec poziomu |

## 🧪 Debugging

Wszystkie zdarzenia wypisują logi do Console z prefixami:
- `[QuestManager]` - zmiany stanów questów
- `[DialogueManager]` - przepływ dialogów
- `[QuestInProgressCondition]` - sprawdzanie warunków

Otwórz Console (Ctrl+Shift+C w edytorze) i obserwuj logi.

## 📋 Checklist Setup

- [ ] Edytować `CreateQuest()` w QuestManager (jeśli chcesz zmienić nazwy)
- [ ] Stworzyć Dialogue Assets dla Wanilli i 3 Companions
- [ ] Ustawić warianty dialogów w inspektorze
- [ ] **WAŻNE:** Upewnić się że questName w dialogach pasuje do nazw w QuestManager
- [ ] Dodać `AddQuestProgress()` w skrypcie zbierania przedmiotów
- [ ] Przetestować cały przepływ
- [ ] Obserwować logi w Console

## 🐛 Najczęstsze Błędy

### Dialog się nie zmienia
**Problem:** questName w wariancie nie pasuje do nazwy w QuestManager
**Rozwiązanie:** Sprawdzić dokładnie (z wielkością liter!):
- W QuestManager: `"Red Quest"`
- W DialogueVariant: `"Red Quest"` (dokładnie to samo!)

### Quest nie przechodzi w stan Ready
**Problem:** `AddQuestProgress()` nie jest wywoływany
**Rozwiązanie:** Dodaj tę linię w skrypcie zbierania przedmiotów

### Action się nie wykonuje
**Problem:** Action Type = None
**Rozwiązanie:** Ustaw właściwą akcję (StartAllQuests, CompleteQuest, itp.)

## 📚 Dodatkowe Materiały

- `QUEST_SYSTEM_GUIDE.md` - Pełna dokumentacja systemu
- `SETUP_GUIDE.md` - Krok po kroku setup
- `QUICKSTART.md` - Szybki start dla dialogów

Wszystko zostało zrobione! 🎉
