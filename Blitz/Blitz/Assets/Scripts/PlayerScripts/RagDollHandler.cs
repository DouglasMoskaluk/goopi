using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollHandler : MonoBehaviour
{
    private Rigidbody[] rigidBodies;
    private Collider[] colliders;

    [SerializeField]
    private Animator anim;

    public bool testRagdoll = false;

    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        DisableRagdoll();
    }

    // Update is called once per frame

    private void DisableRagdoll()
    {
        foreach(var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        colliders[0].enabled = true;
        anim.enabled = true;
    }

    private void EnableRagdoll()
    {
        foreach(var collider in colliders)
        {
            collider.enabled = true;
        }
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
        anim.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            EnableRagdoll();
        }
    }
}
