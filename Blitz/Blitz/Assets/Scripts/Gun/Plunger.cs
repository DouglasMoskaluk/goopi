using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : SpawnableObject
{
    [SerializeField]
    float pullDelay;
    [SerializeField]
    float pullPower;

    internal override void init(int owner)
    {
        //Debug.Log("I'm in the plunger!!!");
        base.init(owner);
        PlayerBodyFSM hit = transform.parent.GetComponent<PlayerBodyFSM>();
        if (hit != null)
        {
            Debug.Log("I've been hit!!! " + hit.name);
            StartCoroutine(pull(hit));
        }
        else StartCoroutine(destruction(5f));
    }

    private IEnumerator pull(PlayerBodyFSM hit)
    {
        yield return new WaitForSeconds(pullDelay);
        hit.newAttacker(Owner);
        PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(Owner);
        Vector3 pullDirection = plr.transform.position - hit.transform.position;
        pullDirection.y += 5;
        hit.addKnockBack(pullDirection * pullPower);
        hit.transitionState(PlayerMotionStates.KnockBack);
        Destroy(gameObject);
    }

    private IEnumerator destruction(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
