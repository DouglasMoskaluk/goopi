using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImpulseGrenade : SpawnableObject
{
    [SerializeField] private bool DISPLAY_DEBUG_RADIUS = false;

    [SerializeField] private float radius = 3f;// the radius of the blast zone
    [SerializeField] private float blastDelay = 0.5f;// the amount of time it takes for the blast to happen once the conditions for it are met
    [SerializeField] private float blastForce = 15f;// the amount of force the player 
    [SerializeField] private bool mapInteractible = false;

    [SerializeField]
    GameObject explodeVFX;

    public GrenadeType type = GrenadeType.Thrown;

    private Rigidbody rb;//this grenades rigidbody

    private bool active = false;// if the blast has been triggered, ie has collided with something

    private Transform whoThrew;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// callback for when the grenade collides with something
    /// decides whether the explode now or later
    /// </summary>
    /// <param name="collision"></param>
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (active) { return; }

    //    if (type == GrenadeType.Dropped && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        armGrenade();
    //    }
    //    else if (type == GrenadeType.Thrown)
    //    {
    //        armGrenade();
    //    }
    //    //Debug.Break();

    //}
    public void setThrowOwner(Transform thrower)
    {
        whoThrew = thrower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active) { return; }

        if (type == GrenadeType.Dropped && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            armGrenade();
        }
        else if (type == GrenadeType.Thrown)
        {
            armGrenade();
        }
    }

    private void armGrenade()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        active = true;
        if (!mapInteractible) explode();
    }

    public void instantExplode(int plr)
    {
        Owner = plr;
        explode();
    }

    /// <summary>
    /// causes the actual explosion of the grenade
    /// </summary>
    private void explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.IMPULSE_DETONATE);
        Instantiate(explodeVFX, transform.position, transform.rotation);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Player"))
            {
                if (hitColliders[i].transform == whoThrew) continue;

                Vector3 dir = ((hitColliders[i].transform.position + Vector3.up * 2) - transform.position).normalized;//the Vector3.up will have to be changed to corrolate with the players height roughly, getting direction to head gives more upwards force which i think feels better ~jordan
                PlayerBodyFSM fsm = hitColliders[i].GetComponent<PlayerBodyFSM>();
                fsm.addKnockBack(dir * blastForce);
                fsm.transitionState(PlayerMotionStates.KnockBack);
                fsm.newAttacker(Owner);
            }
            else if (hitColliders[i].CompareTag("Ragdoll"))
            {
                Vector3 dir = ((hitColliders[i].transform.position + Vector3.up * 2) - transform.position).normalized;//the Vector3.up will have to be changed to corrolate with the players height roughly, getting direction to head gives more upwards force which i think feels better ~jordan
                hitColliders[i].gameObject.GetComponent<Rigidbody>().AddForce(dir * 15, ForceMode.Impulse);

            }
            else if (hitColliders[i].CompareTag("Spinner"))
            {
                hitColliders[i].transform.GetComponent<Spinner>().FastSpin();
            } else if (hitColliders[i].transform.CompareTag("Crate"))
            {
                Vector3 dir = ((hitColliders[i].transform.position + Vector3.up * 2) - transform.position).normalized * 100;//the Vector3.up will have to be changed to corrolate with the players height roughly, getting direction to head gives more upwards force which i think feels better ~jordan
                hitColliders[i].GetComponent<Rigidbody>().AddForce(dir);
                if (hitColliders[i].transform.GetComponent<Crate>() != null) hitColliders[i].transform.GetComponent<Crate>().lastImpulse = Owner;
            }
        }
        Destroy(this.gameObject);
    }

    //Depreciated
    /// <summary>
    /// timer for when the blast should happen
    /// </summary>
    /// <returns></returns>
    private IEnumerator trigger()
    {
        yield return new WaitForSeconds(blastDelay);
        explode();
    }

    public void setDirectionAndSpeed(Vector3 dir, float speed)
    {
        rb.AddForce(dir.normalized * speed, ForceMode.VelocityChange);
    }

    public void setGrenadeType(GrenadeType newType)
    {
        type = newType;
    }

    /// <summary>
    /// displays the blast radius if DEBUG DISPLAY is toggled on
    /// </summary>
    private void OnDrawGizmos()
    {
        if (DISPLAY_DEBUG_RADIUS)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}

public enum GrenadeType
{
    Dropped, Thrown
}
