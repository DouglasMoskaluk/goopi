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
    IEnumerator[] damageTrackers;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        damageTrackers = new IEnumerator[4];
    }


    IEnumerator damageOverTime(int playerID)
    {
        PlayerBodyFSM plrBdy = SplitScreenManager.instance.GetPlayers()[playerID].GetComponent<PlayerBodyFSM>();
        bool notDead = true;
        while (notDead)
        {
            yield return new WaitForSeconds(timeBetweenTriggers);
            if (plrBdy.Health - damage < 0) notDead = false;
            plrBdy.damagePlayer(damage, Owner);
        }
    }


    private void playerDied(EventParams param = new EventParams())
    {
        if (damageTrackers[param.killed] != null) StopCoroutine(damageTrackers[param.killed]);
        damageTrackers[param.killed] = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int id = SplitScreenManager.instance.getPlayerID(other.gameObject);
            //Debug.Log("Starting damage over time for player " + id);
            damageTrackers[id] = damageOverTime(id);
            StartCoroutine(damageTrackers[id]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            int id = SplitScreenManager.instance.getPlayerID(other.gameObject);
            //Debug.Log("Stopping damage over time for player " + id);
            if (damageTrackers[id] != null) StopCoroutine(damageTrackers[id]);
            damageTrackers[id] = null;
        }
    }
}
