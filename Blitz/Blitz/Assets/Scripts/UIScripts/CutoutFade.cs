using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutFade : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float fadeSpeed = 1f;
    private Transform cutoutHolder;
    private int animalUsed = 0;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.speed = fadeSpeed;
        cutoutHolder = transform.GetChild(0);
    }

    public float FadeToBlack()
    {
        SelectNewCutoutAnimal();
        
        switch (animalUsed)
        {
            case 0:
                anim.Play("CutoutFadeToBlackFox", 0, 0);
                break;
            case 1:
                anim.Play("CutoutFadeToBlackAxolotl", 0, 0);
                break;
            case 2:
                anim.Play("CutoutFadeToBlackBadger", 0, 0);
                break;
            case 3:
                anim.Play("CutoutFadeToBlackOtter", 0, 0);
                break;
        }
        return 1 / anim.speed;
    }

    public float FadeToVisible()
    {
        SelectNewCutoutAnimal();

        switch (animalUsed)
        {
            case 0:
                anim.Play("CutoutFadeToVisibleFox", 0, 0);
                break;
            case 1:
                anim.Play("CutoutFadeToVisibleAxolotl", 0, 0);
                break;
            case 2:
                anim.Play("CutoutFadeToVisibleBadger", 0, 0);
                break;
            case 3:
                anim.Play("CutoutFadeToVisibleOtter", 0, 0);
                break;
        }
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

    private void SelectNewCutoutAnimal()
    {
        foreach (Transform child in cutoutHolder)
        {
            child.gameObject.SetActive(false);
        }

        int lastSelected = animalUsed;
        do
        {
            animalUsed = Random.Range(0, cutoutHolder.childCount);
        }
        while (animalUsed == lastSelected);
       
        cutoutHolder.GetChild(animalUsed).gameObject.SetActive(true);
    }
}
