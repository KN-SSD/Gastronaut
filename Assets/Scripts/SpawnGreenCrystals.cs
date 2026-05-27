using System.Collections;
using UnityEngine;

public class SpawnGreenCrystals : MonoBehaviour
{
    [SerializeField] private GameObject greenCrystalPrefab;
    [SerializeField] private string targetQuestName = "Green Quest";
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float timeBetweenSpawns = 2f;

    private void Start()
    {
        StartCoroutine(CheckAndSpawnRoutine());
    }

    private IEnumerator CheckAndSpawnRoutine()
    {
        while (QuestManager.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Quest quest = QuestManager.Instance.GetQuestByName(targetQuestName);
        while (quest == null)
        {
            yield return new WaitForSeconds(0.5f);
            quest = QuestManager.Instance.GetQuestByName(targetQuestName);
        }

        while (true)
        {
            if (quest.questState == QuestState.InProgress)
            {
                Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                randomPosition.y = transform.position.y + 5f;
                Instantiate(greenCrystalPrefab, randomPosition, Quaternion.identity);
            }

            if (quest.questState == QuestState.Completed)
            {
                break;
            }

            // Czekamy ustalony czas przed kolejnym cyklem sprawdzania/spawnowania
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}