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

        EventManager.instance.addListener(Events.onPlayerRespawn, onPlayerDeath);
    }

    private IEnumerator Explode()
    {

        yield return new WaitForSeconds(delay);

        switch (SplitScreenManager.instance.GetPlayers(Owner).playerGun.gunVars.type)
        {
            case Gun.GunType.GOOP:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.GOOP_EXPLOSION);
                break;
            case Gun.GunType.ICE_XBOW:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ICE_EXPLOSION);
                break;
            case Gun.GunType.FISH:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.FISH_EXPLOSION);
                break;
            case Gun.GunType.BOOMSTICK:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_EXPLOSION);
                break;
        }

        //AudioManager.instance.PlaySound(AudioManager.AudioQueue.IMPULSE_DETONATE);
        collider.enabled = true;
        float startRad = collider.radius;
        while (time < explosionTime)
        {
            time += Time.deltaTime;
            collider.radius = Mathf.Lerp(startRad, radius, time/explosionTime);
            yield return null;
        }
        EventManager.instance.removeListener(Events.onRoundEnd, newRound);
        EventManager.instance.removeListener(Events.onPlayerDeath, onPlayerDeath);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerBodyFSM>().damagePlayer(damage, Owner);
        }
    }


    internal void newRound(EventParams param = new EventParams())
    {
        EventManager.instance.removeListener(Events.onRoundEnd, newRound);
        EventManager.instance.removeListener(Events.onPlayerDeath, onPlayerDeath);
        StopCoroutine(explosionCoroutine);
        Destroy(gameObject);
    }


    //TODO: This needs to subscribe to the "OnPlayerDeath" event when we make it
    internal void onPlayerDeath(EventParams param = new EventParams())
    {
        if (transform.parent.GetComponent<PlayerBodyFSM>() != null && param.killed == transform.parent.GetComponent<PlayerBodyFSM>().playerID)
        {
            newRound();
        }
    }


}
