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
    [SerializeField]
    GameObject onExplosionSpawn;

    [SerializeField]
    bool explodable = false;

    IEnumerator explosionCoroutine;

    new SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        explosionCoroutine = Explode();
        if (!explodable)StartCoroutine(explosionCoroutine);
        //EventManager.instance.addListener(Events.onRoundEnd, roundEnd);
        EventManager.instance.addListener(Events.onPlayerRespawn, onPlayerDeath);
    }

    public void explodeNow(int player)
    {
        Owner = player;
        if (explodable)
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {

        yield return new WaitForSeconds(delay);

        if (onExplosionSpawn != null) Instantiate(onExplosionSpawn, transform.position, transform.rotation, transform.parent).transform.localScale = transform.localScale * radius;
        if (!explodable) {
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
        } else
        {
            explodable = false;
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
        EventManager.instance.removeListener(Events.onRoundEnd, roundEnd);
        EventManager.instance.removeListener(Events.onPlayerDeath, onPlayerDeath);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerBodyFSM>().playerID != Owner)
        {
            Vector3 explosionDirection = other.transform.position - transform.position;
            other.GetComponent<PlayerBodyFSM>().damagePlayer(damage, Owner, explosionDirection, transform.position);
        }
    }


    internal override void roundEnd(EventParams param = new EventParams())
    {
        //EventManager.instance.removeListener(Events.onRoundEnd, newRound);
        EventManager.instance.removeListener(Events.onPlayerDeath, onPlayerDeath);
        StopCoroutine(explosionCoroutine);
        base.roundEnd();
    }


    //TODO: This needs to subscribe to the "OnPlayerDeath" event when we make it
    internal void onPlayerDeath(EventParams param = new EventParams())
    {
        if(transform.parent)
        {
            if(transform.parent.GetComponent<PlayerBodyFSM>())
            {
                if (param.killed == transform.parent.GetComponent<PlayerBodyFSM>().playerID)
                {
                    roundEnd();
                }
            }
        }
        //if (transform.parent.GetComponent<PlayerBodyFSM>() != null && param.killed == transform.parent.GetComponent<PlayerBodyFSM>().playerID)
        //{
        //    roundEnd();
        //}
    }


}
