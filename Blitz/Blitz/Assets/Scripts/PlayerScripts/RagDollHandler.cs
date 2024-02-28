using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;

public class RagDollHandler : MonoBehaviour
{

    [SerializeField]
    private float lifetime;

    [SerializeField]
    public Transform camRotatePoint;

    private Transform[] boneList;

    // Start is called before the first frame update
    void Awake()
    {
        boneList = transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    public void AddBulletToRagdoll(GameObject bullet)
    {

    }

    public void DeathForce(Vector3 deathDirection, Vector3 deathPosition)
    {
        deathDirection.Normalize();

        for (int i = 0; i < boneList.Length; i++)
        {
            if (boneList[i].GetComponent<Rigidbody>())
            {
                boneList[i].GetComponent<Rigidbody>().velocity += deathDirection * 15;
            }

        }
    }

    public void InitializeRagdoll(int modelId, int skinNum, Transform[]  bones, Vector3 playerVelocity)
    {
        transform.GetComponent<PlayerModelHandler>().SetRagdollSkin(skinNum);
        transform.GetComponent<PlayerModelHandler>().SetModel(modelId);
        //Transform[] bonelist = transform.GetChild(0).GetComponent<BoneRenderer>().transforms;

        for(int i = 0;i< boneList.Length; i++)
        {
            if (boneList[i].GetComponent<Rigidbody>())
            {
                boneList[i].GetComponent<Rigidbody>().velocity += playerVelocity;
            }

            boneList[i].localPosition = bones[i].localPosition;
            boneList[i].localRotation = bones[i].localRotation;
            boneList[i].localScale = bones[i].localScale;

        }

    }

    // Update is called once per frame

    //public void DisableRagdoll()
    //{
    //    foreach (var rigidBody in rigidBodies)
    //    {
    //        rigidBody.isKinematic = true;
    //    }
    //    foreach (var collider in colliders)
    //    {
    //        collider.enabled = false;
    //    }
    //    colliders[0].enabled = true;
    //    anim.enabled = true;
    //    //fsm.enabled = true;
    //    //gunObject.SetActive(true);

    //    freeLook.m_Follow = gameplayRotatePoint;
    //    freeLook.m_LookAt = gameplayRotatePoint;

    //}

    //public void EnableRagdoll()
    //{
    //    foreach (var collider in colliders)
    //    {
    //        collider.enabled = true;
    //    }
    //    foreach (var rigidBody in rigidBodies)
    //    {
    //        rigidBody.isKinematic = false;
    //    }
    //    anim.enabled = false;
    //    //fsm.enabled = false;
    //    //gunObject.SetActive(false);

    //    freeLook.m_Follow = ragDollRotatePoint;
    //    freeLook.m_LookAt = ragDollRotatePoint;

    //}

    //public void RagDollDeath()
    //{
    //    StartCoroutine("RagDollDeathCoRo");
    //}

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    public void RemoveSelf(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    //IEnumerator RagDollDeathCoRo()
    //{
    //    EnableRagdoll();
    //    yield return new WaitForSeconds(0.5f);
    //    DisableRagdoll();
    //    //fsm.enabled = true;
    //    //transform.position = RespawnManager.instance.getRespawnLocation().position;
    //    //fsm.ragdollDeathEnd();
    //    //transform.position = RespawnManager.instance.getRespawnLocation().position;
    //    yield return null;
    //}

}


