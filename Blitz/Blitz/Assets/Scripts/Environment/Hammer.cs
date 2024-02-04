using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    // Start is called before the first frame update

    
    private Animation anim;

    [SerializeField]
    private Transform hammerHead;

    private Vector3 direction;

    private Vector3 oldPos;

    private Vector3 newPos;

    private int killerID;

    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animation>();    
    }


    public void PlayHammer(int playerID)
    {
        Debug.Log("PLAYER ID" + playerID);
        if(!anim.isPlaying)
        {
            killerID = playerID;
            anim.Play("Hammer");
            StartCoroutine(getVelocity());
        }
        else
        {
            Debug.Log("ALREADY PLAYING HAMMER");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            GameObject plr = other.gameObject;
            if (other.attachedRigidbody != null) plr = other.attachedRigidbody.gameObject;
            PlayerBodyFSM plrFSM = plr.GetComponent<PlayerBodyFSM>();
            if (plrFSM != null)
            {
                plrFSM.damagePlayer(100, killerID);
            }

        }
        else if(other.CompareTag("Ragdoll"))
        {
            Debug.Log(direction);
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 40, ForceMode.Impulse);
            Debug.Log(other.gameObject.name);
            //other.gameObject.GetComponent<RagDollHandler>().MoveRagdoll(direction);
        }

    }

    IEnumerator getVelocity()
    {

        direction = Vector3.zero;

        newPos = new Vector3(hammerHead.position.x, hammerHead.position.y, hammerHead.position.z);

        yield return null;

        while (anim.isPlaying)
        {
            //if(!pastFirst)
            //{
            //    newPos = hammerHead.position;
            //    pastFirst = true;
            //}
            //else
            //{
                oldPos = new Vector3(newPos.x, newPos.y, newPos.z);
                newPos = new Vector3(hammerHead.position.x, hammerHead.position.y, hammerHead.position.z);
                direction = (newPos - oldPos);
            //}

            yield return null;
        }
        //yield return null;
    }


}
