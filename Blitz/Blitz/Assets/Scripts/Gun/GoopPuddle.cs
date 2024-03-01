using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopPuddle : SpawnableObject
{
    [SerializeField]
    private float timeBetweenTriggers = 1f;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float lifeTime = 3f;
    float[] lifetimeDmgTracker;

    private void Start()
    {
        lifetimeDmgTracker = new float[4];
        StartCoroutine(selfDestruct());
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            int id = SplitScreenManager.instance.getPlayerID(other.gameObject);
            lifetimeDmgTracker[id] += Time.deltaTime;
            if (lifetimeDmgTracker[id] > timeBetweenTriggers)
            {
                other.GetComponent<PlayerBodyFSM>().damagePlayer(damage, Owner, Vector3.up, Vector3.zero);
                lifetimeDmgTracker[id] = 0;
            }
        }
    }
}
