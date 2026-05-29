using UnityEngine;
using UnityEngine.Video;

public class MenuVideoScript : MonoBehaviour
{
   private VideoPlayer videoPlayer;
   [SerializeField] private GameObject videoScreen;
   [SerializeField] private GameObject soundPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        Invoke("TurnSoundOn", (float)videoPlayer.clip.length-2.7f);
        Destroy(videoScreen, (float)videoPlayer.clip.length-2.7f);
        Destroy(gameObject, (float)videoPlayer.clip.length-2.7f);
    }

    private void TurnSoundOn()
    {
        soundPlayer.SetActive(true);
    }
}
