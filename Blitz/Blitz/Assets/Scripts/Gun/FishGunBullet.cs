using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGunBullet : Bullet
{
    [SerializeField]
    private float timeBetweenTriggers = 1f;
    [SerializeField]
    private int damage;

    float time = 0;

    private void OnTriggerStay(Collider other)
    {
        if (Vector3.Distance(transform.position, other.ClosestPointOnBounds(transform.position)) < GetComponent<SphereCollider>().radius)
        {
            GameObject plr = other.gameObject;
            if (other.attachedRigidbody != null) plr = other.attachedRigidbody.gameObject;
            RaycastHit hit;
            //Debug.DrawRay(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position)- transform.position , Color.yellow, 10);
            if (Physics.Raycast(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position) - transform.position, out hit, (other.ClosestPointOnBounds(transform.position) - transform.position * 1.1f).magnitude))
            {
                //Debug.Log("Trigger Enter");
                collide(hit);
                collideThisFrame = true;
            }
        } else
        {
            if (other.tag == "Player")
            {
                time += Time.deltaTime;
                if (time > timeBetweenTriggers)
                {
                    other.GetComponent<PlayerBodyFSM>().damagePlayer(damage, bulletVars.owner);
                }
            }
        }
    }
}
