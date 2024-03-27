using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField]
    float recoil = 0.1f;
    [SerializeField]
    float maxRecoil = 0.5f;
    [SerializeField]
    float returnSpeed = 0.05f;

    [SerializeField]
    float maxAngle = 45;
    [SerializeField]
    float angledRecoil = 15;
    [SerializeField]
    float returnAngleSpeed = 20;


    internal void applyRecoil()
    {
        transform.localPosition = transform.localPosition - new Vector3(0, 0, recoil);
        if (transform.localPosition.z < maxRecoil)
        {
            transform.localPosition = new Vector3(0, 0, -maxRecoil);
        }

        transform.localRotation = new Quaternion(transform.localRotation.x - Mathf.Deg2Rad * angledRecoil, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        if (transform.localRotation.x < -Mathf.Deg2Rad * maxAngle)
        {
            transform.localRotation = new Quaternion(-Mathf.Deg2Rad * maxAngle, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        }
    }

    private void Update()
    {
        if (transform.localPosition.z < 0)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, 0, returnSpeed * Time.deltaTime);
            if (transform.localPosition.z > 0)
            {
                transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        if (transform.localRotation.x < 0)
        {
            transform.localRotation = new Quaternion(transform.localRotation.x + Mathf.Deg2Rad * returnAngleSpeed * Time.deltaTime, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
            if (transform.localRotation.x > 0)
            {
                transform.localRotation = Quaternion.identity;
            }
        }
    }
}
