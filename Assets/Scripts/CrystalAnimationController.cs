using UnityEngine;

public class CrystalAnimationController : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(0.4f, 1.2f);
    }
}
