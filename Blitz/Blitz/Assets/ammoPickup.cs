using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Gun gun = other.GetComponent<PlayerBodyFSM>().playerGun;
            if (gun.gunVars.ammo[0] < gun.gunVars.ammo[1])
            {
                gun.gunVars.ammo[0]++;
                Destroy(gameObject);
            }
        }
    }
}
