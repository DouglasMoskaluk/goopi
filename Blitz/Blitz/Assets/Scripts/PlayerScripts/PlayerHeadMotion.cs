using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadMotion : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Transform rotationDirections;
    [SerializeField] private float rotationAmount = 35;
    [SerializeField] private float lerpModifier = 5f;
    private Quaternion ogRotation;

    private void Awake()
    {
        ogRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        Vector3 input = inputHandler.motionInput;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, ogRotation * Quaternion.AngleAxis(-rotationAmount * input.y, Vector3.forward) 
            * Quaternion.AngleAxis(-rotationAmount * input.x, Vector3.right), Time.deltaTime * lerpModifier);

    }

    private float easeOutBack(float num)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(num - 1, 3) + c1 * Mathf.Pow(num - 1, 2);
    }
}
