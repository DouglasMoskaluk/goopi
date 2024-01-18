using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotMachine : MonoBehaviour
{
    [SerializeField] private Transform wheel;
    [SerializeField] private Transform selectedWheel;

    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;

    [SerializeField] private float spinDuration = 10f;
    [SerializeField] private float speed = 1;

    private bool isSpinning = false;

    private void Start()
    {
        StartSelection(1);
    }

    public Coroutine StartSelection(int selectedGun)
    {
        if (isSpinning) return null;

        isSpinning = true;
        return StartCoroutine(SpinWheel());
    }

    private IEnumerator SpinWheel()
    {
        float elapsedTime = 0;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;

            foreach (Transform child in wheel)
            {
                child.localPosition = Vector3.MoveTowards(child.localPosition, bottomPoint.localPosition, speed * Time.deltaTime);
                if (child.localPosition == bottomPoint.localPosition) child.localPosition = topPoint.localPosition;
            }

            yield return null;
        }



    }
}
