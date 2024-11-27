using UnityEngine;

public class AnimationControl : MonoBehaviour
{

    Animator anim;

    public void endShooting() {

        anim = GetComponent<Animator>();

        anim.SetBool("IsShooting", false);

    }

}
