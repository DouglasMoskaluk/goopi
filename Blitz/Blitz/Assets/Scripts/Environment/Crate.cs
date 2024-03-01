using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    internal int damage = 50;
    [SerializeField]
    internal float velocityThreshold = 3;
    internal int lastImpulse = -1;
    Vector3 startingPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingPos = transform.position;
        EventManager.instance.addListener(Events.onRoundEnd, resetPos); 
    }

    private void resetPos(EventParams param = new EventParams())
    {
        transform.position = startingPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && rb.velocity.sqrMagnitude > velocityThreshold * velocityThreshold)
        {
            collision.transform.GetComponent<PlayerBodyFSM>().damagePlayer(damage, lastImpulse, GetComponent<Rigidbody>().velocity, transform.position, -1);
        }
    }
}
