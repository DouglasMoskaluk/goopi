using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private float lifetime;


    private void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.transform.CompareTag("Player"))
        {
            PlayerBodyFSM player = other.transform.GetComponent<PlayerBodyFSM>();
            player.refillHealth();
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
