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

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HideCredits(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
