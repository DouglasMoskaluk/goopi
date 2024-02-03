using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    [SerializeField]
    private Gun.GunType gunPickup;
    [SerializeField]
    private float returnHeight = 0;
    private static Vector3 startPos = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GunManager.instance.assignGun(SplitScreenManager.instance.getPlayerID(other.gameObject), (int)gunPickup-1);
            Destroy(transform.parent.gameObject);
        }
    }

    private void Start()
    {
        if (startPos == Vector3.zero)
        {
            startPos = transform.parent.position;
        } else if (transform.parent.position.y < returnHeight)
        {
            transform.parent.position = startPos;
        }
    }
}
