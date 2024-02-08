using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAOE : GoopPuddle
{
    private void OnTriggerEnter(Collider other)
    {
        Transform bul = transform.parent;
        bul.GetComponent<Rigidbody>().velocity = bul.GetComponent<Rigidbody>().velocity.normalized * bul.GetComponent<Bullet>().bulletVars.minSpeed;
        //base.onTriggerEnter(other);
    }
}
