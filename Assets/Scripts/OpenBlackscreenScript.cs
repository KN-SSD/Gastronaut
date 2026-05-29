using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenBlackscreenScript : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    void Start()
    {
        Transform child = transform.Find("OpenImage");
        if (child != null)
            fadeImage = child.GetComponent<Image>();

        if (fadeImage != null)
        {
            fadeImage.enabled = true;
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float time = 0;
        Color c = fadeImage.color;

        while (time<fadeDuration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, time/fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        if (fadeImage != null) fadeImage.enabled = false;
    }
}