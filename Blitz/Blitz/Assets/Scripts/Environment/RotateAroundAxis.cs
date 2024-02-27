using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    [SerializeField] private bool shouldRotate = true;
    [SerializeField] private Vector3 rotationAxis;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        if (shouldRotate) transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
