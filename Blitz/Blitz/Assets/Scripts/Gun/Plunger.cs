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
    AnimationCurve heightCurve;
    [SerializeField]
    float CratePullPower = 350;

    [SerializeField]
    AnimationCurve curve;

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
        else if (transform.parent.tag == "Ragdoll")
        {
            StartCoroutine(pullPlayer(null));
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
        if (transform.parent.tag == "Player")
        {
            hit.newAttacker(Owner);
            PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(Owner);
            Vector3 pullDirection = (plr.transform.position - hit.transform.position).normalized;

            Debug.Log("Before: "+pullDirection.magnitude);
            //pullDirection = pullDirection * ((115 - plr.Health) * (115 - plr.Health));

            pullDirection *= pullPower;

            Debug.Log("Pull Power added: "+pullDirection.magnitude);

            pullDirection = pullDirection * curve.Evaluate(1-(hit.Health/100.0f));

            Debug.Log("Evaluated curve: " +pullDirection.magnitude);

            pullDirection.y += heightPull * heightCurve.Evaluate(1-(hit.Health/100.0f));

            Debug.Log("Height: " + pullDirection.magnitude);


            hit.addKnockBack(pullDirection);
            hit.transitionState(PlayerMotionStates.KnockBack);
            
        }
        else
        {
            Debug.Log("RAGDOLL PULL");
            PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(Owner);
            Vector3 pullDirection = (plr.transform.position - transform.position).normalized;
            //pullDirection.y += heightPull;
            transform.root.GetComponent<RagDollHandler>().pullRagdoll(pullDirection);
        }

        Destroy(gameObject);
    }

    public void startRagdollPull()
    {
        StartCoroutine(pullPlayer(null));
    }

    private IEnumerator pullCrate()
    {
        yield return new WaitForSeconds(pullDelay);
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLUNGER_PULL);
        PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(Owner);
        Vector3 pullDirection = plr.transform.position - transform.parent.position;
        pullDirection.y += 5;
        transform.parent.GetComponent<Rigidbody>().AddForce(pullDirection.normalized * CratePullPower);
        if (transform.parent.GetComponent<Crate>() != null) transform.parent.GetComponent<Crate>().lastImpulse = Owner;
        Destroy(gameObject);
    }
    //here's where I make my MOVE

    private IEnumerator destruction(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
