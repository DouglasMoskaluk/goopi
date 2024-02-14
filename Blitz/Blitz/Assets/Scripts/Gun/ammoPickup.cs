using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : SpawnableObject
{
    [SerializeField]
    private float deathBarrier = 0;
    private float deathCheck = 1;

    private void Start()
    {
        StartCoroutine(VoidCheck());
    }

    private IEnumerator VoidCheck()
    {
        while(true) 
        {
            yield return new WaitForSeconds(deathCheck);
            if (transform.position.y < deathBarrier)
            {
                transform.position = RespawnManager.instance.getRespawnLocation().position;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Gun gun = other.GetComponent<PlayerBodyFSM>().playerGun;
            if (gun.gunVars.type == Gun.GunType.NERF && gun.gunVars.ammo[0] < gun.gunVars.ammo[1])
            {
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.NERF_RELOAD);
                gun.gunVars.ammo[0]++;
                Destroy(gameObject);
            }
        }
    }
}
