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


    internal void applyRecoil()
    {
        transform.localPosition = transform.localPosition - new Vector3(0, 0, recoil);
        if (transform.localPosition.z < maxRecoil)
        {
            transform.localPosition = new Vector3(0, 0, -maxRecoil);
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
    }
}
