using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTutorialAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float howManyAnimLoops = 3f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public float playGunTutorialSequence()
    {
        anim.SetInteger("GunSelected", GunManager.instance.GunUsed);
        return 1f * howManyAnimLoops / anim.speed;
    }

    public void ResetTutorial()
    {
        anim.SetInteger("GunSelected", -1);
    }
}
