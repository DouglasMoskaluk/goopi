using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMashImage : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float waitTime = 0.5f;
    private Image uiImage;
    bool usedSprite = false;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(SwapImages());
    }

    private IEnumerator SwapImages()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            if (usedSprite) uiImage.sprite = sprites[0]; else uiImage.sprite = sprites[1];
            usedSprite = !usedSprite;
        }
    }

    public void StopImageSwap()
    {
        StopAllCoroutines();
    }

}
