using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunTutorialAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Image img;
    [SerializeField] private float tutPlayTime = 1.5f;
    [SerializeField] private float howManyAnimLoops = 3f;
    [SerializeField] private Sprite[] firstFrames;


    public void setInitialFrameEvent(EventParams param = new EventParams())
    {
        Debug.Log("inside set frame event");
        //anim.enabled = false;
        img.sprite = firstFrames[GunManager.instance.GunUsed];
        //anim.enabled = true;
    }

    public void setInitialFrame()
    {
        Debug.Log(img == null);
        img.sprite = firstFrames[GunManager.instance.GunUsed];
    }

    public float playGunTutorialSequence()
    {
        anim.SetInteger("GunSelected", GunManager.instance.GunUsed);
        if (GunManager.instance.GunUsed == 3)//plunger gun, has a different animation play time
        {
            return tutPlayTime * howManyAnimLoops / 2 * anim.speed;
        }
        return tutPlayTime * howManyAnimLoops / anim.speed;
    }

    public void resetTutorial()
    {
        anim.SetInteger("GunSelected", -1);
    }
}
