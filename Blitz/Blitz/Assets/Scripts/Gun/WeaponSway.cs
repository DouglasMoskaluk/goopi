using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private bool shouldSway = true;

    [SerializeField] private float posYOffset = 0.025f;
    [SerializeField] private float negYOffset = -0.01f;
    [SerializeField] private float lerpModifier = 1;
    
    private int direction = 1;

    private Vector3 startPoint;

    private Vector3 top;
    private Vector3 bottom;

    [Tooltip("-1 -> bottom, 1 -> top")] private int target = 1;

    private void Start()
    {
        startPoint = transform.localPosition;
        top = startPoint + Vector3.up * posYOffset;
        bottom = startPoint + Vector3.up * negYOffset;
        
    }

    public void Update()
    {
        if (shouldSway)
        {
            moveWeapon();

            checkDirection();
        }
    }

    private void moveWeapon()
    {
        Vector3 target = (direction == 1) ? top : bottom;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, lerpModifier * Time.deltaTime);
    }

    private void checkDirection()
    {
        if ((direction == 1 && transform.localPosition.Equals(top)) || (direction == -1 && transform.localPosition.Equals(bottom)))
        {
            direction *= -1;
        }
    }

    public void setStartPoint(Vector3 newStart)
    {
        startPoint = newStart;
        transform.localPosition = startPoint;
    }

}
