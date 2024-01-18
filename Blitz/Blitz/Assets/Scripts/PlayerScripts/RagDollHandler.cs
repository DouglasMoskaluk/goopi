using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RagDollHandler : MonoBehaviour
{

    private Rigidbody[] rigidBodies;
    private Collider[] colliders;

    private Animator anim;

    private CinemachineFreeLook freeLook;

    [SerializeField]
    private Transform ragDollRotatePoint;

    [SerializeField]
    private Transform gameplayRotatePoint;
    //public bool testRagdoll = false;

    [SerializeField]
    private GameObject[] testObjectArray;

    private PlayerBodyFSM fsm;

    [SerializeField]
    private GameObject gunObject;

    // Start is called before the first frame update
    void Start()
    {


        freeLook = GetComponentInChildren<CinemachineFreeLook>();
        colliders = GetComponentsInChildren<Collider>();
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        fsm = GetComponentInChildren<PlayerBodyFSM>();

        foreach (var GameObject in testObjectArray)
        {
            GameObject.layer = gameObject.layer;
        }


        DisableRagdoll();
    }

    public void InitializeRagdoll(int playerId)
    {

    }

    // Update is called once per frame

    public void DisableRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        colliders[0].enabled = true;
        anim.enabled = true;
        //fsm.enabled = true;
        //gunObject.SetActive(true);

        freeLook.m_Follow = gameplayRotatePoint;
        freeLook.m_LookAt = gameplayRotatePoint;

    }

    public void EnableRagdoll()
    {
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
        anim.enabled = false;
        //fsm.enabled = false;
        //gunObject.SetActive(false);

        freeLook.m_Follow = ragDollRotatePoint;
        freeLook.m_LookAt = ragDollRotatePoint;

    }

    public void RagDollDeath()
    {
        StartCoroutine("RagDollDeathCoRo");
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    IEnumerator RagDollDeathCoRo()
    {
        EnableRagdoll();
        yield return new WaitForSeconds(0.5f);
        DisableRagdoll();
        //fsm.enabled = true;
        //transform.position = RespawnManager.instance.getRespawnLocation().position;
        //fsm.ragdollDeathEnd();
        //transform.position = RespawnManager.instance.getRespawnLocation().position;
        yield return null;
    }

}


