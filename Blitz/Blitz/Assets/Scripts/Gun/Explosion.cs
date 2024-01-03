using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : SpawnableObject
{
    [SerializeField]
    internal float delay = 0;
    [SerializeField]
    internal float radius = 5f;
    [SerializeField]
    internal float explosionTime = 0.2f;
    internal float time = 0;
    [SerializeField]
    internal int damage = 35;

    IEnumerator explosionCoroutine;

    SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        explosionCoroutine = Explode();
        StartCoroutine(explosionCoroutine);
        EventManager.instance.addListener(Events.onRoundEnd, newRound);
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
        float startRad = collider.radius;
        while (time < explosionTime)
        {
            time += Time.deltaTime;
            collider.radius = Mathf.Lerp(startRad, radius, time/explosionTime);
            yield return null;
        }
        EventManager.instance.removeListener(Events.onRoundEnd, newRound);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerBodyFSM>().damagePlayer(damage, Owner);
        }
    }


    internal void newRound()
    {
        EventManager.instance.removeListener(Events.onRoundEnd, newRound);
        StopCoroutine(explosionCoroutine);
        Destroy(gameObject);
    }


    //TODO: This needs to subscribe to the "OnPlayerDeath" event when we make it
    internal void onPlayerDeath(PlayerBodyFSM playerDied)
    {
        if (playerDied.gameObject == transform.parent)
        {
            newRound();
        }
    }


}
