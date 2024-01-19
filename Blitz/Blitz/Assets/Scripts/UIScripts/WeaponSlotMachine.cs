using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotMachine : MonoBehaviour
{
    [SerializeField] private RectTransform wheel;
    [SerializeField] private RectTransform selectedWheel;

    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;

    [SerializeField] private float spinDuration = 10f;

    [SerializeField] private float speed = 1f;

    [SerializeField] private float slowedDownFinalSpeed = 750f;
    [SerializeField] private float slowDownSpeedAmount = 150f;

    public float usedSpeed;

    private Vector3 Image1BottomVisible;

    private bool isSpinning = false;

    private void Start()
    {
        usedSpeed = speed;
        Image1BottomVisible = wheel.GetChild(0).localPosition;
        //StartSelection(1);
    }

    public Coroutine StartSelection(int selectedGun)
    {
        if (isSpinning) return null;

        isSpinning = true;
        return StartCoroutine(SpinWheel(selectedGun));
    }

    public void ResetSpinner()
    {
        
    }

    private IEnumerator SpinWheel(int selectedGun)
    {
        //Debug.Log(selectedGun);
        float elapsedTime = 0;
        Vector3 finalImagePos = Image1BottomVisible + (-Vector3.up * 256 * selectedGun);
        //Debug.Log(finalImagePos);

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

        //Debug.Log("finished elapsed time");

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


        float maxDist = Vector3.Distance(topPoint.position, finalImagePos);
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

    }
}
