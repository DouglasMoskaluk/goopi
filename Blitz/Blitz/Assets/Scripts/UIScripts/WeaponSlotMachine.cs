using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class WeaponSlotMachine : MonoBehaviour
{
    [SerializeField] private RectTransform wheel;

    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;

    [SerializeField] private float spinDuration = 10f;
    [SerializeField] private float gunVisibleDuration = 1.5f;

    [SerializeField] private float speed = 1f;

    [SerializeField] private float slowedDownFinalSpeed = 750f;
    [SerializeField] private float slowDownSpeedAmount = 150f;

    //[SerializeField] private VideoPlayer vidPlayer;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float fadeOutTime = 1.5f;

    [SerializeField] private Sprite[] stamps;
    [SerializeField] private Image stampImage;

    public float usedSpeed;

    private Vector3 Image1BottomVisible;

    private bool isSpinning = false;

    [SerializeField] private Transform resetChild0;
    [SerializeField] private Transform resetChild1;

    private float stampInitScale;
    [SerializeField] private float finalStampSize;
    [SerializeField] private float stampDuration = 1;

    [SerializeField] private Animator rouletteAnimator;

    private void Start()
    {
        usedSpeed = speed;
        Image1BottomVisible = wheel.GetChild(0).localPosition;
        stampInitScale = stampImage.transform.localScale.x;
    }

    public Coroutine StartSelection(int selectedGun)
    {
        if (isSpinning) return null;

        isSpinning = true;
        ResetSpinner();
        return StartCoroutine(SpinWheel(selectedGun));
    }

    public void ResetSpinner()
    {
        usedSpeed = speed;
        wheel.GetChild(0).position = resetChild0.position;
        wheel.GetChild(1).position = resetChild1.position;
        stampImage.gameObject.SetActive(false);
        //vidPlayer.frame = 0;**
        stampImage.transform.localScale = Vector3.one * stampInitScale;
    }

    private IEnumerator SpinWheel(int selectedGun)
    {
        rouletteAnimator.speed = 1;
        rouletteAnimator.Play("Spin", 0, 0);

        float elapsedTime = 0;
        Vector3 finalImagePos = Image1BottomVisible - (Vector3.up * 348.5f * (5 - selectedGun));//347

        yield return new WaitForSecondsRealtime(4.2f);//time it takes for otter to pull lever
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROULETTE_START);


        //vidPlayer.Pause(); **
        rouletteAnimator.speed = 0;
        

        // spin until the end of the specified spinning duration
        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            foreach (RectTransform child in wheel)
            {
                child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, usedSpeed * Time.unscaledDeltaTime);
                if (child.localPosition == bottomPoint.localPosition) child.localPosition = topPoint.localPosition;
            }

            yield return null;
        }

        //spin until the first child is on the very top
        bool getOut = false;
        while (true)
        {

            foreach (RectTransform child in wheel)
            {
                child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, usedSpeed * Time.unscaledDeltaTime);
                if (child.localPosition == bottomPoint.localPosition)
                {
                    child.localPosition = topPoint.localPosition;
                    if (child == wheel.GetChild(0))
                    {
                        getOut = true;
                    }
                }
            }

            if (getOut) break;

            yield return null;
        }

        //spin until selected gun is visible
        Transform child1 = wheel.GetChild(0);
        while (true)
        {

            usedSpeed = Mathf.MoveTowards(usedSpeed, slowedDownFinalSpeed, Time.unscaledDeltaTime * slowDownSpeedAmount);

            foreach (RectTransform child in wheel)
            {
                if (child == child1) child.localPosition = Vector3.MoveTowards(child.localPosition, finalImagePos, usedSpeed * Time.unscaledDeltaTime);
                else child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, usedSpeed * Time.unscaledDeltaTime);
            }

            if (wheel.GetChild(0).localPosition == finalImagePos) { break; }

            yield return null;
        }

        AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROULETTE_SELECT);

        yield return new WaitForSecondsRealtime(gunVisibleDuration);

        //stamp animation
        if (selectedGun < 5)
        {
            stampImage.sprite = stamps[selectedGun];
            stampImage.gameObject.SetActive(true);
            float stampElapsedTime = 0;
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_OBLITERATED);
            while (stampElapsedTime < stampDuration)
            {
                stampElapsedTime += Time.unscaledDeltaTime;

                stampImage.transform.localScale = Vector3.Lerp(stampImage.transform.localScale, Vector3.one * finalStampSize, stampElapsedTime / stampDuration);

                yield return null;
            }
        }

        //vidPlayer.Play();**
        rouletteAnimator.speed = 1;



        yield return new WaitForSecondsRealtime(2.0f);

        isSpinning = false;
        rouletteAnimator.speed = 0;
    }
}
