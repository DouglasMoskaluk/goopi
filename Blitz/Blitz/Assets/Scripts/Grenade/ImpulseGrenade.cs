using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseGrenade : MonoBehaviour
{
    [SerializeField] private bool DISPLAY_DEBUG_RADIUS = false;

    [SerializeField] private float radius = 3f;// the radius of the blast zone
    [SerializeField] private float blastDelay = 0.5f;// the amount of time it takes for the blast to happen once the conditions for it are met
    [SerializeField] private float blastForce = 15f;// the amount of force the player 

    private Rigidbody rb;//this grenades rigidbody

    private bool active = false;// if the blast has been triggered, ie has collided with something

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// callback for when the grenade collides with something
    /// decides whether the explode now or later
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (active) { return; }

        rb.constraints = RigidbodyConstraints.FreezePosition;
        active = true;
        if (collision.gameObject.CompareTag("Player"))
        {
           explode();
        }
        else
        {
            StartCoroutine(trigger());
        }
       
    }

    /// <summary>
    /// 
    /// </summary>
    private void explode()
    {
        List<PlayerBodyFSM> targets = new List<PlayerBodyFSM>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Player"))
            {
                Vector3 dir = ((hitColliders[i].transform.position + Vector3.up * 2) - transform.position).normalized;//the Vector3.up will have to be changed to corrolate with the players height roughly, getting direction to head gives more upwards force which i think feels better ~jordan
                hitColliders[i].GetComponent<PlayerBodyFSM>().addKnockBack(dir * blastForce);
                //Debug.DrawRay(transform.position, dir * 3);
                //Debug.Break();
            }
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// timer for when the blast should happen
    /// </summary>
    /// <returns></returns>
    private IEnumerator trigger()
    {
        yield return new WaitForSeconds(blastDelay);
        explode();
    }

    /// <summary>
    /// displays the blast radius if DEBUG DISPLAY is toggled on
    /// </summary>
    private void OnDrawGizmos()
    {
        if (DISPLAY_DEBUG_RADIUS) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}
