using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunTutorialAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Image img;
    //[SerializeField] private float amountOfTimeToSeeTutAnim = 
    [SerializeField] private Sprite[] firstFrames;

    public void setInitialFrame()
    {
        if (GunManager.instance.GunUsed == 5) return;
        img.sprite = firstFrames[GunManager.instance.GunUsed];
    }

    public float playGunTutorialSequence()
    {
        anim.SetInteger("GunSelected", GunManager.instance.GunUsed);
        return 3;
    }

    public void resetTutorial()
    {
        anim.SetInteger("GunSelected", -1);
    }
}
