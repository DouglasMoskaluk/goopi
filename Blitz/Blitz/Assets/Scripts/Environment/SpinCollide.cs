using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCollide : MonoBehaviour
{

    //[SerializeField]
    //private float flingForce;

    private Vector3 direction = Vector3.zero;

    private Vector3 newPos = Vector3.zero;

    private Vector3 oldPos;

    private Spinner spinner;

    private void Awake()
    {
       newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
       spinner = transform.parent.parent.GetComponent<Spinner>();
    }

    private void FixedUpdate()
    {
        oldPos = new Vector3(newPos.x, newPos.y, newPos.z);
        newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        direction = (newPos - oldPos);
        //direction.Normalize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            direction.Normalize();
            direction.y = 0.4f;
            //Vector3 dir = ((other.transform.position + Vector3.up * 2) - transform.position).normalized;//the Vector3.up will have to be changed to corrolate with the players height roughly, getting direction to head gives more upwards force which i think feels better ~jordan
            PlayerBodyFSM fsm = other.GetComponent<PlayerBodyFSM>();
            fsm.addKnockBack(direction * spinner.flingForce);
            fsm.transitionState(PlayerMotionStates.KnockBack);
        }
    }
}
