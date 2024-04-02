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

    private List<GameObject> bodyBullets;

    private GameObject[] stuckBullets;

    [SerializeField]
    private GameObject stars;

    // Start is called before the first frame update
    void Awake()
    {
        boneList = transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    public void Stars()
    {
        stars.SetActive(true);
    }

    public void pullRagdoll(Vector3 direction)
    {

        for (int i = 0; i < boneList.Length; i++)
        {
            if (boneList[i].GetComponent<Rigidbody>())
            {
                boneList[i].GetComponent<Rigidbody>().velocity += direction * 30;
            }

        }

    }

    public void AddBulletToRagdoll(GameObject bullet, int killer)
    {
        if (!bullet.name.Equals("Bomb(clone)"))
        {
            int closestBone = 0;

            bullet.transform.localScale = new Vector3(1, 1, 1);

            for (int i = 0; i < boneList.Length; i++)
            {
                if (boneList[i].GetComponent<Rigidbody>())
                {
                    if (Vector3.Distance(bullet.transform.position, boneList[i].position) <= Vector3.Distance(bullet.transform.position, boneList[closestBone].position))
                    {
                        closestBone = i;
                    }

                }

            }

            GameObject newBullet = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation, boneList[closestBone]);
            newBullet.transform.localScale = new Vector3(0.007474666f, 0.007474666f, 0.007474666f);

            GrowGameObject growth = newBullet.GetComponent<GrowGameObject>();
            if (growth != null)
            {
                float newLife = growth.lifeTime;
                float newBulEndSize = growth.getScale();
                newBullet.GetComponent<GrowGameObject>().SetValues(0.007474666f, 0.007474666f * newBulEndSize, newLife);
                newBullet.GetComponent<Explosion>().Owner = killer;
            }
            else if (newBullet.GetComponent<Plunger>())
            {
                newBullet.transform.GetChild(2).transform.gameObject.SetActive(false);
                newBullet.GetComponent<Plunger>().Owner = killer;

                newBullet.GetComponent<Plunger>().startRagdollPull();

            }

            Destroy(bullet);
        }

    }

    public void SetBulletArrayList(List<GameObject> bullets, List<int> killers)
    {
        //bodyBullets = bullets;

        for(int i = 0; i < bullets.Count;i++)
        {
            AddBulletToRagdoll(bullets[i], killers[i]);
        }

    }

    public void DeathForce(Vector3 deathDirection, Vector3 killThingPos)
    {
            deathDirection.Normalize();

            int closestBone = 0;
            //float closestDistance = 0.0f;

            for (int i = 0; i < boneList.Length; i++)
            {
                if (boneList[i].GetComponent<Rigidbody>())
                {
                    if (Vector3.Distance(killThingPos, boneList[i].position) <= Vector3.Distance(killThingPos, boneList[closestBone].position))
                    {
                        closestBone = i;
                    }

                }

            }

        boneList[closestBone].GetComponent<Rigidbody>().velocity += deathDirection * 100;


        //if(bulletType.Equals("Ice Bullet(clone)"))
        //{
        //    Debug.Log("NEW BULLET ON RAGDOLL");
        //    GameObject newBullet = Instantiate(stuckBullets[0], killThingPos, Quaternion.identity, boneList[closestBone]);
        //    newBullet.transform.localScale = new Vector3(0.007474666f, 0.007474666f, 0.007474666f);

        //    if (newBullet.GetComponent<GrowGameObject>())
        //    {
        //        newBullet.GetComponent<GrowGameObject>().SetValues(0.007474666f, 0.007474666f * 2, 0.0f);
        //    }

        //    //Destroy(deathObject);
        //}
        //if (deathObject.name.Equals("Ice Bullet(Clone)"))
        //{
        //    Debug.Log("NEW BULLET ON RAGDOLL");
        //    GameObject newBullet = Instantiate(stuckBullets[0], deathObject.transform.position, deathObject.transform.rotation, boneList[closestBone]);
        //    newBullet.transform.localScale = new Vector3(0.007474666f, 0.007474666f, 0.007474666f);

        //    if (newBullet.GetComponent<GrowGameObject>())
        //    {
        //        float newLife = deathObject.GetComponent<GrowGameObject>().lifeTime;
        //        newBullet.GetComponent<GrowGameObject>().SetValues(0.007474666f, 0.007474666f * 2, newLife);
        //    }

        //    //Destroy(deathObject);
        //}





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


