using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private float lifetime;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private int ammoPickedUp;

    private void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    private void FixedUpdate()
    {
        transform.Rotate(0.0f, rotationSpeed, 0.0f);
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.transform.CompareTag("Player"))
        {
            PlayerBodyFSM player = other.transform.GetComponent<PlayerBodyFSM>();
            player.refillHealth();
            if (player.playerGun.gunVars.type == Gun.GunType.NERF)
            {
                player.playerGun.gunVars.ammo[0] += ammoPickedUp;
                if (player.playerGun.gunVars.ammo[0] > player.playerGun.gunVars.ammo[1])
                {
                    player.playerGun.gunVars.ammo[0] = player.playerGun.gunVars.ammo[1];
                }
                player.playerGun.gunVars.canShoot = true;
            } else 
            {
                player.playerGun.instantReload();
            }
            Destroy(gameObject);
        }

    }

    public void RemoveSelf(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
