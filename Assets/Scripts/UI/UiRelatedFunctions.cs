using UnityEngine;
using UnityEngine.SceneManagement;

public class UiRelatedFunctions : MonoBehaviour
{
   public void ChangeScene(string sceneName)
   {
       SceneManager.LoadScene(sceneName);
   }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

}
