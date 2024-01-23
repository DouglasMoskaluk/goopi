using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    [SerializeField]
    private Gun.GunType gunPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GunManager.instance.assignGun(SplitScreenManager.instance.getPlayerID(other.gameObject), (int)gunPickup-1);
            Destroy(transform.parent.gameObject);
        }
    }
}
