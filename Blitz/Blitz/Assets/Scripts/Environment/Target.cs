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

    public void BulletHit(int killerID)
    {
        //Debug.Log("TARGET PLAYER " + killerID);

        //Debug.Log("hit target");
        newEvent.Invoke(killerID);

    }

    // Update is called once per frame
    void Update()
    {
        if(Hammer.instance.GetHammerPlaying())
        {
            targetStates[1].SetActive(true);
            targetStates[0].SetActive(false);
        }
        else
        {
            targetStates[0].SetActive(true);
            targetStates[1].SetActive(false);
        }
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Bullet")
    //    {
    //        Debug.Log("TEST");
    //        //newEvent.Invoke();
    //    }
    //}

}
