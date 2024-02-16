using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAOEPulse : MonoBehaviour
{
    [SerializeField]
    float pulseLength = 0.7f;
    [SerializeField]
    GameObject pulsewaveExplosion;
    
    private void OnTriggerEnter(Collider other)
    {
        Transform bul = transform.parent;
        bul.GetComponent<Rigidbody>().velocity = bul.GetComponent<Rigidbody>().velocity.normalized * bul.GetComponent<Bullet>().bulletVars.minSpeed;
        //base.onTriggerEnter(other);
    }

    private void Start()
    {
        StartCoroutine(pulse());
    }

    IEnumerator pulse()
    {
        int owner = transform.parent.GetComponent<Bullet>().bulletVars.owner;
        while (true)
        {
            yield return new WaitForSeconds(pulseLength);
            Instantiate(pulsewaveExplosion, transform.position, transform.rotation, transform.parent.parent).GetComponent<Explosion>().init(owner);
        }
    }
}
