using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTransitionMotionManager : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    public float playRoundTransitionMotion()
    {
        anim.Play("RoundTransitionMotionMotion", 0, 0);
        return 2 / anim.speed;
    }

    public void ResetMotion()
    {
        anim.Play("RoundTransitionMotionStart", 0, 0);
    }
}
