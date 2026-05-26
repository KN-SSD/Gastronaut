public class Quest
{
    public string questName;
    public bool isQuestActive;
    public bool isQuestCompleted;
    public int currentProgress;
    public int requiredProgress;

    public Quest(string name, int requiredProgress)
    {
        this.questName = name;
        this.requiredProgress = requiredProgress;
        this.currentProgress = 0;
        this.isQuestActive = false;
        this.isQuestCompleted = false;
    }
}
