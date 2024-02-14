using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : SpawnableObject
{
    [SerializeField]
    private float deathBarrier = 0;
    private float deathCheck = 1;
    [SerializeField]
    private float radius;

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
                float x;
                float y;
                do
                {
                    x = Random.Range(-radius, radius);
                    y = Random.Range(-radius, radius);
                } while (Mathf.Abs(x)+Mathf.Abs(y) < radius);
                transform.position = ModifierManager.instance.vars.centralLocation.position + new Vector3(x, 0, y);//RespawnManager.instance.getRespawnLocation().position;
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
