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
    [SerializeField] private float speed = 1;

    private Vector3 Image1BottomVisible;

    private bool isSpinning = false;

    private void Start()
    {
        Image1BottomVisible = wheel.GetChild(0).localPosition;
        Debug.Log(Image1BottomVisible);
        StartSelection(1);
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
        Debug.Log(selectedGun);
        float elapsedTime = 0;
        Vector3 finalImagePos = Image1BottomVisible + (-Vector3.up * 256 * selectedGun);
        Debug.Log(finalImagePos);

        // spin until the end of the specified spinning duration
        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;

            foreach (RectTransform child in wheel)
            {
                child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, speed * Time.deltaTime);
                if (child.localPosition == bottomPoint.localPosition) child.localPosition = topPoint.localPosition;
            }

            yield return null;
        }

        Debug.Log("finished elapsed time");

        //spin until the first child is on the very top
        bool getOut = false;
        while (true)
        {

            foreach (RectTransform child in wheel)
            {
                child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, speed * Time.deltaTime);
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


        while (true)
        {
            foreach (RectTransform child in wheel)
            {
                if (child == wheel.GetChild(0)) child.localPosition = Vector3.MoveTowards(child.localPosition, finalImagePos, speed * Time.deltaTime);
                else child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, speed * Time.deltaTime);
            }

            if (wheel.GetChild(0).localPosition == finalImagePos) { Debug.Log(wheel.GetChild(0).localPosition); break; }

            yield return null;
        }

    }
}
