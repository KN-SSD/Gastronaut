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
        Invoke("TurnSoundOn", (float)videoPlayer.clip.length-3.9f);
        Destroy(videoScreen, 3.7f);//(float)videoPlayer.clip.length-3.9f);
        Destroy(gameObject, 3.7f);//(float)videoPlayer.clip.length-3.9f);
    }

    private void TurnSoundOn()
    {
        soundPlayer.SetActive(true);
    }
}
