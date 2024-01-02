using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : SpawnableObject
{
    [SerializeField]
    internal float delay = 0;
    internal float radius = 5f;
    internal float explosionTime = 0.5f;
    internal float time = 0;

    internal int damage = 35;

    SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
        float startRad = collider.radius;
        while (time < explosionTime)
        {
            time += Time.deltaTime;
            collider.radius = Mathf.Lerp(startRad, radius, time);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerBodyFSM>().damagePlayer(damage, Owner);
        }
    }


}
