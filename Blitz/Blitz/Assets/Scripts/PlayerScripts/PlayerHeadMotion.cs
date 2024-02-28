using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadMotion : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Transform rotationDirections;
    [SerializeField] private float rotationAmount = 35;
    private Vector3 ogRotation;

    private void Awake()
    {
        ogRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        Vector3 input = inputHandler.motionInput;

        transform.localEulerAngles = ogRotation + (rotationDirections.forward * rotationAmount * input.y) + (rotationDirections.up * rotationAmount * input.x);
    }
}
