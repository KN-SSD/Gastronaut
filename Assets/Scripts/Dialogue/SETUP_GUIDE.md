# Praktyczne Ustawienia - Krok po Kroku

## Scenariusz: Wanilla daje questy, Companionowie je odbierają

### Krok 1: Zmień CreateQuest w QuestManager

Skonwertuj tę część w `QuestManager.cs`:

```csharp
public void CreateQuest()
{
    // Format: Quest(nazwa, wymagane_przedmioty, quest_giver, quest_receiver)
    quests.Add(new Quest("Red Quest", 3, "Wanilla", "Red Companion"));
    quests.Add(new Quest("Green Quest", 5, "Wanilla", "Green Companion"));
    quests.Add(new Quest("Blue Quest", 3, "Wanilla", "Blue Companion"));
}
```

Już zmieniliśmy to dla Ciebie! ✅

### Krok 2: Utwórz Dialogue Asset dla Wanilli

W Unity:
1. Kliknij RMB w `Assets/Dialogue/` (lub gdzie masz dialogi)
2. Create → Dialogue (jeśli masz custom scriptable object)
3. Lub po prostu edytuj istniejący Dialogue Asset na obiekcie Wanilli

Ustaw:
- **NPC Name**: Wanilla
- **Wariant 0**:
  - Sentences: `["Cześć, witaj!", "Muszę Ci prosić o coś ważnego..."]`
  - Condition Type: `AlwaysTrue`
  - Action Type: `StartAllQuests`
  
- **Wariant 1**:
  - Sentences: `["Jak idą prace na questach?"]`
  - Condition Type: `AllQuestsCompleted`
  - Action Type: `LevelComplete`

### Krok 3: Utwórz Dialogue dla Red Companion'a

Nowy Dialogue Asset:
- **NPC Name**: Red Companion
- **Wariant 0** (Nieukończony quest):
  - Sentences: `["Jeszcze nie masz kryształów...", "Wróć jak zbierzesz 3 sztuki"]`
  - Condition Type: `QuestInProgress`
  - Action Type: `None`

- **Wariant 1** (Quest gotów):
  - Sentences: `["Masz je! Dziękuję!", "To ogromnie mi pomoże!"]`
  - Condition Type: `QuestReady`
  - Action Type: `CompleteQuest`

- **Wariant 2** (Quest ukończony):
  - Sentences: `["Dzięki jeszcze raz za kryształy!"]`
  - Condition Type: `QuestCompleted`
  - Action Type: `None`

### Krok 4: Przypisz Dialogue do NPC w scenie

Na obiekcie Wanilli (`NPCDialogue` component):
- Drag Dialogue Asset "Wanilla" do pola Dialogue

Na obiekcie Red Companion:
- Drag Dialogue Asset "Red Companion" do pola Dialogue

(Tak samo dla Green i Blue)

### Krok 5: Implementacja zbierania przedmiotów

Gdziekolwiek gracz zbiera przedmioty (np. w `PickUpAndHoldItem.cs`):

```csharp
void PickUpItem()
{
    // ... twój kod ...
    
    // Dodaj postęp do questa
    if (pickedItem.color == Color.red)
        QuestManager.Instance.AddQuestProgress("Red Quest");
    else if (pickedItem.color == Color.green)
        QuestManager.Instance.AddQuestProgress("Green Quest");
    else if (pickedItem.color == Color.blue)
        QuestManager.Instance.AddQuestProgress("Blue Quest");
}
```

## 🎮 Jak gracz będzie grać

1. **Zaczyna grę** - mówi do Wanilli
2. **Wanilla**: "Muszę Ci prosić..." (otrzymuje questy)
3. **UI się zmienia** - pokazuje 3 questy z 0/3 przedmiotów
4. **Gracz zbiera** - przesuwają się liczniki (0/3 → 1/3 → 2/3 → 3/3)
5. **UI**: "Red Quest: Ready! Talk to Red Companion" (zieloną czcionką)
6. **Gracz idzie do Red Companion'a**
   - Jeśli mnie ma: "Jeszcze nie masz..."
   - Jeśli ma: "Dziękuję! Quest complete!"
7. **Powtarza dla Green i Blue**
8. **Po wszystkich 3**: Wanilla ma nowy dialog: "Dziękuję za całą pomoc! Koniec poziomu!"

## 🧪 Testy

### Test 1: Czy dialogi się zmieniają?
1. Rozmów z Wanillą
2. Zbierz 3 czerwone przedmioty
3. Rozmów z Red Companion'em
4. Powinieneś zobaczyć inne zdania

### Test 2: Czy questy trackują postęp?
1. Otwórz Console
2. Szukaj `[QuestManager]` logów
3. Powinien pokazywać "Red Quest: 1/3", "Red Quest: 2/3" itp.

### Test 3: Czy UI się aktualizuje?
1. Obserwuj tekst questów w UI
2. Powinien zmienić kolor na zielony gdy quest jest Ready
3. Powinien pokazać "Complete!" gdy ukończony

## ⚡ Szybki Checklist

- [ ] Zmienić `CreateQuest()` w QuestManager
- [ ] Utworzyć Dialogue Asset dla Wanilli
- [ ] Utworzyć Dialogue Assets dla 3 Companion'ów
- [ ] Przypisać Dialogue Assets do NPC'ów w scenie
- [ ] Dodać `AddQuestProgress()` w miejscu gdzie zbiera się przedmioty
- [ ] Przetestować cały przepływ
- [ ] Wyłapać i naprawić błędy

## 🔗 Powiązane Klasy

- `Quest.cs` - struktura questa ze stanami
- `QuestManager.cs` - zarządzanie questami
- `Dialogue.cs` - warianty dialogów z warunkami i akcjami
- `DialogueManager.cs` - wyświetlanie dialogów
- `NPCDialogue.cs` - triggering dialogów na NPC'ach
