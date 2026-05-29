using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> quests = new List<Quest>();
    [SerializeField] private TextMeshProUGUI questRedText;
    [SerializeField] private TextMeshProUGUI questGreenText;
    [SerializeField] private TextMeshProUGUI questBlueText;

    [SerializeField] private NPCDialogue questReqirementsMet;

    [SerializeField] private GameObject redCrystal;
    [SerializeField] private GameObject greenCrystal;
    [SerializeField] private GameObject blueCrystal;
    [SerializeField] private float delay = 3.0f;

    private string targetQuestName;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            CreateQuest();

            if (redCrystal != null) redCrystal.SetActive(false);
            if (greenCrystal != null) greenCrystal.SetActive(false);
            if (blueCrystal != null) blueCrystal.SetActive(false);
        }
        else
            Destroy(gameObject);
    }

    public void CreateQuest()
    {
        quests.Add(new Quest("Red Quest", 3, "Baise De", "Hang Se"));
        quests.Add(new Quest("Green Quest", 7, "Baise De", "Lan Se"));
        quests.Add(new Quest("Blue Quest", 3, "Baise De", "Luse De"));
        
        Debug.Log("[QuestManager] Questy utworzone");
    }

    public bool HasQuestStarted(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && quest.questState == QuestState.InProgress)
            return true;
        return false;
    }

    /// <summary>
    /// Uruchamia wszystkie questy
    /// </summary>
    public void StartAllQuests()
    {
        foreach (var quest in quests)
        {
            if (quest.questState == QuestState.NotStarted)
            {
                quest.questState = QuestState.InProgress;
                questRedText.gameObject.SetActive(true);
                questGreenText.gameObject.SetActive(true);
                questBlueText.gameObject.SetActive(true);
                Debug.Log($"[QuestManager] Quest '{quest.questName}' uruchomiony");
            }
        }
        UpdateQuestUI();
    }

    /// <summary>
    /// Zwraca questę po nazwie
    /// </summary>
    public Quest GetQuestByName(string questName)
    {
        return quests.Find(q => q.questName == questName);
    }

    /// <summary>
    /// Oznacza questę jako ukończoną
    /// </summary>
    public void CompleteQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null)
        {
            quest.Complete();
            UpdateQuestUI();
            Debug.Log($"[QuestManager] Quest '{questName}' ukończony!");

            CrystalActivated(questName);

            // Sprawdź czy wszystkie questy są ukończone
            if (AreAllQuestsCompleted())
            {
                Debug.Log("[QuestManager] WSZYSTKIE QUESTY UKOŃCZONE!");
            }
        }
        else
        {
            Debug.LogWarning($"[QuestManager] Quest '{questName}' nie znaleziony!");
        }
    }

    /// <summary>
    /// Dodaje postęp do questa
    /// </summary>
    public bool AddQuestProgress(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && (quest.questState == QuestState.InProgress || quest.questState == QuestState.Ready))
        {
            quest.currentProgress++;
            quest.UpdateState();
            
            Debug.Log($"[QuestManager] Postęp '{questName}': {quest.currentProgress}/{quest.requiredProgress} (stan: {quest.questState})");
            
            UpdateQuestUI();


            if(quest.questState == QuestState.Ready)
                questReqirementsMet.TriggerDialogue(quest.GetQuestReceiverName());

            return true;
        }
        
        Debug.LogWarning($"[QuestManager] Nie można dodać postępu do '{questName}' (stan: {quest?.questState})");
        return false;
    }

    /// <summary>
    /// Sprawdza czy wszystkie questy są ukończone
    /// </summary>
    public bool AreAllQuestsCompleted()
    {
        foreach (var quest in quests)
        {
            if (quest.questState != QuestState.Completed)
                return false;
        }
        return true;
    }

    private void UpdateQuestUI()
    {
        UpdateQuestTextUI("Red Quest", questRedText);
        UpdateQuestTextUI("Green Quest", questGreenText);
        UpdateQuestTextUI("Blue Quest", questBlueText);
    }

    private void UpdateQuestTextUI(string questName, TextMeshProUGUI textUI)
    {
        if (textUI == null) return;

        Quest quest = GetQuestByName(questName);
        if (quest == null) return;

        switch (quest.questState)
        {
            // case QuestState.NotStarted:
            //     textUI.text = $"{questName}: Not started";
            //     break;
                
            case QuestState.InProgress:
                textUI.text = $"{quest.currentProgress}/{quest.requiredProgress}";
                break;
                
             case QuestState.Ready:
                textUI.text = $"<color=green>{quest.currentProgress}/{quest.requiredProgress}</color>";
                break;
                
             case QuestState.Completed:
                textUI.text = $"<color=green>√</color>";
                break;
        }
    }

    private void CrystalActivated(string questName)
    {
        targetQuestName = questName;

        Invoke(nameof(ActivateWithDelay), delay);
    }

    private void ActivateWithDelay()
    {
        GameObject currentCrystal = null;

        if (targetQuestName == "Red Quest") currentCrystal = redCrystal;
        else if (targetQuestName == "Green Quest") currentCrystal = greenCrystal;
        else if (targetQuestName == "Blue Quest") currentCrystal = blueCrystal;

        if (currentCrystal != null) currentCrystal.SetActive(true);
    }
}

