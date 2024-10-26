using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadMotion : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Transform rotationDirections;
    [SerializeField] private float rotationAmountX = 35;
    [SerializeField] private float rotationAmountY = 35;
    [SerializeField] private float lerpModifier = 5f;
    private Quaternion ogRotation;

    private void Awake()
    {
        ogRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        Vector3 input = inputHandler.motionInput;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, ogRotation * Quaternion.AngleAxis(-rotationAmountY * input.y, Vector3.forward) 
            * Quaternion.AngleAxis(-rotationAmountX * input.x, Vector3.right), Time.deltaTime * lerpModifier);

    }
}
