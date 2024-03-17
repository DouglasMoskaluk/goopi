using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutFade : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float fadeSpeed = 1f;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.speed = fadeSpeed;
    }

    public float FadeToBlack()
    {
        anim.Play("CutoutFadeToBlack", 0, 0);
        return 1 / anim.speed;
    }

    public float FadeToVisible()
    {
        anim.Play("CutoutFadeToVisible", 0, 0);
        return 1 / anim.speed;
    }

    public void SetVisibility(bool onOff)
    {
        gameObject.SetActive(onOff);
    }

    public void FadeToVisibleInstant()
    {
        anim.Play("CutoutFadeToVisibleInstant", 0, 0);
    }

    public void FadeToBlackInstant()
    {
        anim.Play("CutoutFadeToBlackInstant", 0, 0);
    }
}
