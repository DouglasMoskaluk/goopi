using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopPuddle : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenTriggers = 0.5f;
    [SerializeField]
    private int damage;
    private float lifeTime = 3f;
    private float time = 0;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Stay " + other.name);
        if (other.gameObject.tag == "Player")
        {
            PlayerBodyFSM plr = other.GetComponent<PlayerBodyFSM>();
            Debug.Log(time);
            time += Time.deltaTime;
            if (time >= timeBetweenTriggers)
            {
                time = 0;
                if (plr == null)
                {
                    plr.alterHealth(-damage);
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        time = 0;
    }
}
