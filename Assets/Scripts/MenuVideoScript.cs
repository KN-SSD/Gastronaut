using UnityEngine;
using UnityEngine.Video;

public class MenuVideoScript : MonoBehaviour
{
   private VideoPlayer videoPlayer;
   [SerializeField] private GameObject videoScreen;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        Destroy(videoScreen, (float)videoPlayer.clip.length-2.7f);
        Destroy(gameObject, (float)videoPlayer.clip.length-2.7f);
    }
}
