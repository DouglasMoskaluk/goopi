using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : SpawnableObject
{
    [SerializeField]
    float pullDelay;
    [SerializeField]
    float pullPower;
    [SerializeField]
    float heightPull = 5;
    [SerializeField]
    float CratePullPower = 350;

    internal override void init(int owner)
    {
        //Debug.Log("I'm in the plunger!!!");
        base.init(owner);
        transform.GetComponentInChildren<PlungerCord>().init(Owner);
        PlayerBodyFSM hit = transform.parent.GetComponent<PlayerBodyFSM>();
        if (hit != null)
        {
            //Debug.Log("I've been hit!!! " + hit.name);
            StartCoroutine(pullPlayer(hit));
        }
        else if (transform.parent.tag == "Crate")
        {
            StartCoroutine(pullCrate());
        }
        else
        {
            StartCoroutine(destruction(5f));
            transform.GetComponentInChildren<LineRenderer>().gameObject.SetActive(false);
        }
    }

    private IEnumerator pullPlayer(PlayerBodyFSM hit)
    {
        yield return new WaitForSeconds(pullDelay);
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLUNGER_PULL);
        hit.newAttacker(Owner);
        PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(Owner);
        Vector3 pullDirection = plr.transform.position - hit.transform.position;
        pullDirection = pullDirection * pullPower * (100 - plr.Health) * (100 - plr.Health);
        pullDirection.y += heightPull;
        hit.addKnockBack(pullDirection);
        hit.transitionState(PlayerMotionStates.KnockBack);
        Destroy(gameObject);
    }

    private IEnumerator pullCrate()
    {
        yield return new WaitForSeconds(pullDelay);
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLUNGER_PULL);
        PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(Owner);
        Vector3 pullDirection = plr.transform.position - transform.parent.position;
        pullDirection.y += 5;
        transform.parent.GetComponent<Rigidbody>().AddForce(pullDirection.normalized * CratePullPower);
        Destroy(gameObject);
    }

    private IEnumerator destruction(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
