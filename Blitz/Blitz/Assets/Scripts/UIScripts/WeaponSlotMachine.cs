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

    [SerializeField] private float speed = 1f;

    [SerializeField] private float slowedDownFinalSpeed = 750f;
    [SerializeField] private float slowDownSpeedAmount = 150f;

    [SerializeField] private VideoPlayer vidPlayer;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float fadeOutTime = 1.5f;

    [SerializeField] private Sprite[] stamps;
    [SerializeField] private Image stampImage;

    private CanvasGroup cGroup;

    private float usedSpeed;

    private Vector3 Image1BottomVisible;

    private bool isSpinning = false;

    private Vector3 resetChild0;
    private Vector3 resetChild1;

    private float stampInitScale;
    [SerializeField] private float finalStampSize;
    [SerializeField] private float stampDuration = 1;

    private void Awake()
    {
        usedSpeed = speed;
        Image1BottomVisible = wheel.GetChild(0).localPosition;
        resetChild0 = wheel.GetChild(0).localPosition;
        resetChild1 = wheel.GetChild(1).localPosition;
        cGroup = GetComponent<CanvasGroup>();
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
        wheel.GetChild(0).localPosition = resetChild0;
        wheel.GetChild(1).localPosition = resetChild1;
        stampImage.gameObject.SetActive(false);
        vidPlayer.frame = 0;
    }

    private IEnumerator SpinWheel(int selectedGun)
    {
        //second 3.59, frame 14 | seconds 6.84, frame 20, 

        vidPlayer.frame = 0;
        vidPlayer.Play();
        //yield return null;
        //vidPlayer.Pause();

        //fade slot machine in
        float timeElapsedFade = 0;
        while (timeElapsedFade < fadeTime)
        {
            timeElapsedFade += Time.unscaledDeltaTime;

            cGroup.alpha = (timeElapsedFade / fadeTime);
            yield return null;
        }

        float elapsedTime = 0;
        Vector3 finalImagePos = Image1BottomVisible - (Vector3.up * 348.5f * (5 - selectedGun));//347
        //vidPlayer.Play();

        yield return new WaitForSecondsRealtime(3.7f - fadeTime);

        vidPlayer.Pause();
        //vidPlayer.frame = 20;

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

            usedSpeed = Mathf.MoveTowards(usedSpeed, slowedDownFinalSpeed, Time.deltaTime * slowDownSpeedAmount);

            foreach (RectTransform child in wheel)
            {
                if (child == child1) child.localPosition = Vector3.MoveTowards(child.localPosition, finalImagePos, usedSpeed * Time.unscaledDeltaTime);
                else child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, usedSpeed * Time.unscaledDeltaTime);
            }

            if (wheel.GetChild(0).localPosition == finalImagePos) { break; }

            yield return null;
        }

        vidPlayer.Pause();
        yield return new WaitForSecondsRealtime(1.5f);

        if (selectedGun < 5) stampImage.sprite = stamps[selectedGun];
        stampImage.gameObject.SetActive(true);
        float stampElapsedTime = 0;
        while (stampElapsedTime < stampDuration)
        {
            stampElapsedTime += Time.unscaledDeltaTime;

            stampImage.transform.localScale = Vector3.Lerp(stampImage.transform.localScale, Vector3.one * finalStampSize, stampElapsedTime / stampDuration);

            yield return null;
        }

        vidPlayer.Play();
        yield return new WaitForSecondsRealtime(2.0f);

        float timeElapsedFadeOut = 0;
        while (timeElapsedFadeOut < fadeOutTime)
        {
            timeElapsedFadeOut += Time.unscaledDeltaTime;

            cGroup.alpha = 1 - (timeElapsedFadeOut / fadeOutTime);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.2f);

        isSpinning = false;
    }
}
