using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    internal int damage;
    [SerializeField]
    internal float velocityThreshold = 3;
    internal int lastImpulse = -1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && rb.velocity.sqrMagnitude > velocityThreshold * velocityThreshold)
        {
            collision.transform.GetComponent<PlayerBodyFSM>().damagePlayer(damage, lastImpulse);
        }
    }
}
