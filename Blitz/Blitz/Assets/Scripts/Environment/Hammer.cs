using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    // Start is called before the first frame update
    public static Hammer instance;

    private Animation anim;

    [SerializeField]
    private Transform hammerHead;

    private Vector3 direction;

    private Vector3 oldPos;

    private Vector3 newPos;

    private int killerID;

    void Start()
    {
        if (instance == null) instance = this;
        anim = transform.GetChild(0).GetComponent<Animation>();    
    }


    public void PlayHammer(int playerID)
    {
        if(!anim.isPlaying)
        {

            AudioManager.instance.PlaySound(AudioManager.AudioQueue.HAMMER_SWING);

            killerID = playerID;
            anim.Play("Hammer");
            StartCoroutine(getVelocity());
        }
        else
        {
            //Debug.Log("ALREADY PLAYING HAMMER");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            //Debug.Log("hit player");
            GameObject plr = other.gameObject;
            if (other.attachedRigidbody != null) plr = other.attachedRigidbody.gameObject;
            PlayerBodyFSM plrFSM = plr.GetComponent<PlayerBodyFSM>();
            if (plrFSM != null)
            {
                plrFSM.playerUI.Hammered();
                plrFSM.damagePlayer(100, killerID, direction, transform.position);
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.HAMMER_KILL);
            }

        }
        else if(other.CompareTag("Ragdoll"))
        {
            //Debug.Log(direction);
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 25, ForceMode.Impulse);
            //other.gameObject.GetComponent<RagDollHandler>().MoveRagdoll(direction);
        }

    }

    public bool GetHammerPlaying()
    {
        return anim.isPlaying;
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
