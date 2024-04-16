using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    UnityEvent<int> newEvent;

    [SerializeField]
    private GameObject[] targetStates;

    [SerializeField]
    private bool isHammer = true;

    [SerializeField]
    private bool canTouch = false;

    [SerializeField]
    internal int owner;

    internal bool CanTouch() { return canTouch; }

    public void BulletHit(int killerID)
    {
        //Debug.Log("TARGET PLAYER " + killerID);

        //Debug.Log("hit target");
        newEvent.Invoke(killerID);

    }

    private void OnEnable()
    {
        owner = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Hammer.instance.GetHammerPlaying() && isHammer)
        {
            targetStates[1].SetActive(true);
            targetStates[0].SetActive(false);
        }
        else if (isHammer)
        {
            targetStates[0].SetActive(true);
            targetStates[1].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTouch)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                int playerNum = other.transform.GetComponent<PlayerBodyFSM>().playerID;
                newEvent.Invoke(playerNum);
            }
            else if (other.gameObject.CompareTag("Ragdoll"))
            {
                newEvent.Invoke(-1);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if ((!canTouch && GetComponent<Rigidbody>().velocity.sqrMagnitude > 25))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                int playerNum = collision.transform.GetComponent<PlayerBodyFSM>().playerID;
                newEvent.Invoke(playerNum);
            }
            else if (collision.gameObject.CompareTag("Ragdoll"))
            {
                newEvent.Invoke(-1);
            }
            else if (owner != -1) newEvent.Invoke(owner);
        }
    }

}
