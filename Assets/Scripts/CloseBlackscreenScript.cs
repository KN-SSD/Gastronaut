using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class CloseBlackscreenScript : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    void Start()
    {
        Transform child = transform.Find("CloseImage");
        if (child != null)
            fadeImage = child.GetComponent<Image>();

        if (fadeImage != null)
        {
            fadeImage.enabled = false;
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public void StartClosingScreen()
    {
        if (fadeImage != null)
        {
            fadeImage.enabled = true;
            StartCoroutine(FadeOut());
        }
    }

 private IEnumerator FadeOut()
{
    float time = 0;
    Color c = fadeImage.color;

    while (time < fadeDuration)
    {
        time += Time.deltaTime;
        
        float t = time / fadeDuration; 

        float easedT = Mathf.Sin(t * Mathf.PI * 0.5f);

        c.a = easedT; 
        
        fadeImage.color = c;
        yield return null;
    }
    c.a = 1f;
    fadeImage.color = c;
    
    ChangeScene();
}

    private void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public float GetFadeTime()
    {
        return fadeDuration;
    }
}