using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToRocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.Instance.IsLevelFinished())
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.MarkCurrentDialogueAsSceneChanger();

                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

                DialogueManager.Instance.LoadSceneAndWait(nextSceneIndex);
            }
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}