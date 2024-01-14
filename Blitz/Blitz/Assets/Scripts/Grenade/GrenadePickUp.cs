using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickUp : MonoBehaviour
{
    [SerializeField] private GameObject grenadeVisual;
    [SerializeField] private float respawnTime;
    private bool canPickUp = true;


    private void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, ResetGrenade);
    }

    private void OnTriggerStay(Collider other)
    {

        if (!other.transform.CompareTag("Player") || !canPickUp) return;
        
        PlayerGrenadeThrower grenThrow = other.transform.GetComponent<PlayerGrenadeThrower>();

        if (grenThrow == null) return;

        if (grenThrow.HeldGrenadeCount >= grenThrow.MaxHeldGrenades) return;

        grenThrow.addGrenade(1);

        canPickUp = false;
        grenadeVisual.SetActive(false);

        StartCoroutine(Respawn());


    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        ResetGrenade();

    }

    public void ResetGrenade(EventParams param = new EventParams())
    {
        canPickUp = true;
        grenadeVisual.SetActive(true);
    }
}
