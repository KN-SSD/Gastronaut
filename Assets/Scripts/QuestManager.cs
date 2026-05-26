using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> quests = new List<Quest>();
        [SerializeField] private TextMeshProUGUI questRedText;
        [SerializeField] private TextMeshProUGUI questGreenText;
        
        [SerializeField] private TextMeshProUGUI questBlueText;


    void Awake()
    {
        if(Instance == null)
            {
            Instance = this;
            CreateQuest();
            }
        else
            Destroy(gameObject);
    }

    public void CreateQuest()
    {
        quests.Add(new Quest("Red Quest", 3));
        quests.Add(new Quest("Green Quest", 5));
        quests.Add(new Quest("Blue Quest", 3));
    }

    public void StartQuestes()
    {
        foreach (var quest in quests)
        {
            quest.isQuestActive = true;
                UpdateQuestUI();
        }
    }

    private void UpdateQuestUI()
    {
        foreach (var quest in quests)
        {
            if (quest.isQuestActive)
            {
                if (quest.questName == "Red Quest")
                    questRedText.text = $"{quest.questName}: {quest.currentProgress}/{quest.requiredProgress}";
                else if (quest.questName == "Green Quest")
                    questGreenText.text = $"{quest.questName}: {quest.currentProgress}/{quest.requiredProgress}";
                else if (quest.questName == "Blue Quest")
                    questBlueText.text = $"{quest.questName}: {quest.currentProgress}/{quest.requiredProgress}";
            }
            else if (quest.isQuestCompleted)
            {
                if (quest.questName == "Red Quest")
                    questRedText.text = $"{quest.questName}: Collected, Talk to NPC!";
                else if (quest.questName == "Green Quest")
                    questGreenText.text = $"{quest.questName}: Collected, Talk to NPC!";
                else if (quest.questName == "Blue Quest")
                    questBlueText.text = $"{quest.questName}: Collected, Talk to NPC!";
            }
        }
    }

    public bool AddQuestProgress(string questName)
    {
        Quest quest = quests.Find(q => q.questName == questName);
        if (quest != null && quest.isQuestActive)
        {
            quest.currentProgress++;
            CheckIfQuestCompleted();
            UpdateQuestUI();
            return true;
        }
        return false;
    }

    private void CheckIfQuestCompleted()
    {
        foreach (var quest in quests)
        {
            if (quest.isQuestActive && quest.currentProgress >= quest.requiredProgress)
            {
                quest.isQuestActive = false;
                quest.isQuestCompleted = true;
                Debug.Log($"Quest '{quest.questName}' completed!");
                UpdateQuestUI();
            }
        }
    }

}