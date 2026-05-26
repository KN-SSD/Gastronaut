# System Questów i Dialogów - Pełny Poradnik

## 📋 Przegląd Systemu

### Nowa struktura questów

Każdy quest ma teraz **stan**:
- **NotStarted** - Quest jeszcze się nie rozpoczął
- **InProgress** - Quest aktywny, ale gracz nie ma wszystkich przedmiotów
- **Ready** - Gracz ma wszystkie przedmioty, może iść do NPC'a
- **Completed** - Quest ukończony

### Przepływ questów

```
1. Gracz rozmawia z Quest Giverem (np. Wanilla)
   ↓
2. Wanilla daje 3 questy (stan zmienia się na InProgress)
   ↓
3. Gracz zbiera przedmioty do Czerwonego Questa
   ↓
4. UI pokazuje "Red Quest: Ready! Talk to Red Companion"
   ↓
5. Gracz idzie do Red Companion'a
   ↓
6. Red Companion sprawdza czy gracz ma wymagane przedmioty
   - Jeśli nie: "Jeszcze nie czas..."
   - Jeśli tak: "Dziękuję!" i oznacza questę jako Completed
   ↓
7. Gracz wraca do Wanilli
   ↓
8. Wanilla: "Dziękuję, że pomogłeś!"
   ↓
9. Jeśli WSZYSTKIE questy ukończone: nowy dialog "Dziękuję za całą pomoc! Koniec poziomu!"
```

## 🎯 Jak to ustawić w inspectorze

### 1. Quest Giver (Wanilla)

Na obiekcie Wanilli, w komponencie `NPCDialogue`, ustaw dialogi:

```
Wariant 1 (Początek - przed questami):
  Condition: AlwaysTrue
  Sentences:
    - "Cześć! Muszę Ci prosić o coś ważnego..."
    - "Musisz zbierać rzeczy do trzech zadań"
  Action: StartAllQuests

Wariant 2 (Gdy questy są w trakcie):
  Condition: QuestInProgress (questName: "Red Quest")
  Sentences:
    - "Pamiętaj, zbieram czerwone kryształy!"
  Action: None

Wariant 3 (Gdy wszystkie questy ukończone):
  Condition: AllQuestsCompleted
  Sentences:
    - "Dziękuję za całą pomoc!"
    - "Uratowałeś nas wszystkich!"
  Action: LevelComplete
```

### 2. Quest Receiver (Red Companion)

Na obiekcie Red Companion'a:

```
Wariant 1 (Jeszcze nie jest gotów):
  Condition: QuestInProgress (questName: "Red Quest")
  Sentences:
    - "Jeszcze nie masz wszystkich kryształów..."
    - "Wróć gdy zbierzesz 3 sztuki"
  Action: None

Wariant 2 (Quest ready - gracz ma przedmioty):
  Condition: QuestReady (questName: "Red Quest")
  Sentences:
    - "Świetnie! Masz wszystkie kryształy!"
    - "Dziękuję za pomoc!"
  Action: CompleteQuest (questName: "Red Quest")

Wariant 3 (Quest już ukończony):
  Condition: QuestCompleted (questName: "Red Quest")
  Sentences:
    - "Dziękuję jeszcze raz!"
  Action: None
```

## 🔧 Warunki dialogowe

| Warunek | Co robi |
|---------|---------|
| `AlwaysTrue` | Zawsze wyświetl |
| `QuestStarted` | Jeśli jakiś quest jest aktywny |
| `QuestInProgress` | Jeśli konkretny quest jest w stadzie InProgress |
| `QuestReady` | Jeśli konkretny quest ma wszystkie przedmioty |
| `QuestCompleted` | Jeśli konkretny quest jest ukończony |
| `AllQuestsCompleted` | Jeśli ALL questy są completed |

## ⚙️ Akcje dialogowe

| Akcja | Co robi |
|-------|---------|
| `None` | Brak akcji |
| `StartAllQuests` | Uruchamia wszystkie questy |
| `CompleteQuest` | Oznacza questę jako ukończoną |
| `LevelComplete` | Koniec poziomu |

## 📊 Używanie QuestManager w kodzie

```csharp
// Dodaj postęp do questa
QuestManager.Instance.AddQuestProgress("Red Quest");

// Pobierz questę
Quest quest = QuestManager.Instance.GetQuestByName("Red Quest");

// Sprawdź stan questa
if (quest.questState == QuestState.Ready)
    Debug.Log("Quest ready!");

// Sprawdź czy wszystkie questy ukończone
bool allDone = QuestManager.Instance.AreAllQuestsCompleted();
```

## 🐛 Debugowanie

### Przydatne logi

Wszystkie akcje wypisują logi z prefiksem `[` do Console. Szukaj:
- `[QuestManager]` - zmiany stanów
- `[Dialogue]` - które warianty się wybierają
- `[DialogueManager]` - przepływ dialogów

### Testowanie

W inspektorze, na Dialogue Asset'cie, ustaw wszystkie warianty na `AlwaysTrue` aby testować każdy z osobna.

## 📝 Dodawanie nowych warunków

### Przykład: Warunek "Item w Inventory"

1. Stwórz plik `Assets/Scripts/Dialogue/DialogueConditions/HasItemCondition.cs`:

```csharp
[System.Serializable]
public class HasItemCondition : IDialogueCondition
{
    [SerializeField] private string itemName;

    public bool IsMet()
    {
        return PlayerInventory.Instance.HasItem(itemName);
    }
}
```

2. Dodaj do enum w `Dialogue.cs`:
```csharp
public enum DialogueConditionType
{
    // ...
    HasItem,
}
```

3. Dodaj do `DialogueConditionFactory.cs`:
```csharp
case DialogueConditionType.HasItem:
    return new HasItemCondition().IsMet();
```

## Najczęstsze problemy

### Problem: Quest nie zmienia się na Ready
**Rozwiązanie:** Sprawdź czy `AddQuestProgress()` jest wywoływany z prawidłową nazwą questa. Nazwa musi dokładnie pasować!

### Problem: Dialog się nie zmienia
**Rozwiązanie:** Sprawdź w jakiej kolejności są warianty. Warianty są sprawdzane od końca do początku (od najbardziej specjalnych do ogólnych).

### Problem: Action się nie wykonuje
**Rozwiązanie:** Sprawdź czy Action Type jest ustawiony (nie None) i czy akcja jest obsługiwana w `DialogueActionFactory`.
