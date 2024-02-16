using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAOEPulse : MonoBehaviour
{
    [SerializeField]
    float pulseLength = 0.7f;
    [SerializeField]
    GameObject pulsewaveExplosion;
    [SerializeField]
    float sizeScaler = 0.4f;
    
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
            GameObject go = Instantiate(pulsewaveExplosion, transform.position, transform.rotation, transform.parent.parent);
            go.transform.localScale = transform.parent.localScale * sizeScaler;
            go.GetComponent<Explosion>().init(owner);

        }
    }
}
