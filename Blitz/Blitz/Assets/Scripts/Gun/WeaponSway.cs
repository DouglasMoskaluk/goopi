using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private bool shouldSway = true;

    [SerializeField] private float posYOffset = 0.025f;
    [SerializeField] private float negYOffset = 0.01f;
    [SerializeField] private float lerpModifier = 1;
    private Vector3 startPoint;
    private int direction = 1;

    private float timeSinceLastDirectionChange = 0;

    private void Start()
    {
        startPoint = transform.localPosition;
        ChangeDirection();
    }

    public void Update()
    {
        if (shouldSway)
        {
            float offsetValue = (direction >= 0) ? posYOffset : negYOffset;
            transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + Vector3.up * offsetValue, lerpModifier * Time.deltaTime);

            if (transform.localPosition.y >= startPoint.y + posYOffset || transform.localPosition.y <= startPoint.y - negYOffset)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeDirection()
    {

        direction *= -1;
    }

    public void SetStartPoint(Vector3 newStart)
    {
        startPoint = newStart;
        transform.localPosition = startPoint;
    }

}
