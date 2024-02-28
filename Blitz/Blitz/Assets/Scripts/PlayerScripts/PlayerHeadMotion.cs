using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadMotion : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Transform rotationDirections;
    [SerializeField] private float rotationAmount = 35;
    private Quaternion ogRotation;

    private void Awake()
    {
        ogRotation = transform.localRotation;
        Debug.Log(ogRotation);
    }

    private void Update()
    {
        Vector3 input = inputHandler.motionInput;

        transform.rotation = ogRotation * Quaternion.AngleAxis(rotationAmount * input.y, rotationDirections.right)
            * Quaternion.AngleAxis(rotationAmount * input.x, rotationDirections.forward);
    }
}
