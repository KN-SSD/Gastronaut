using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

        while (time<fadeDuration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, time/fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }

    public float GetFadeTime()
    {
        return fadeDuration;
    }
}