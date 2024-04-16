using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    internal int damage = 50;
    [SerializeField]
    internal float velocityThreshold = 3;
    internal int lastImpulse = -1;
    Vector3 startingPos;
    [SerializeField]
    private bool dmg = true;
    [SerializeField]
    private bool metal = false;
    private float chanceNoPlaySound = 0.4f;
    private bool playSounds = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingPos = transform.position;
        EventManager.instance.addListener(Events.onRoundEnd, resetPos);
        StartCoroutine(cooldown());
    }

    private IEnumerator cooldown()
    {
        yield return new WaitForSeconds(1);
        playSounds = true;
    }

    private void Update()
    {
        if (transform.position.y < -4)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void resetPos(EventParams param = new EventParams())
    {
        GetComponent<MeshRenderer>().enabled = true;
        transform.position = startingPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > velocityThreshold && playSounds && Random.Range(0, 1.0f) < chanceNoPlaySound) { 
            if (metal ) AudioManager.instance.PlaySound(AudioManager.AudioQueue.COLLISION_METAL);
            else if (!metal) AudioManager.instance.PlaySound(AudioManager.AudioQueue.COLLISION_WOOD);
        }
        if (collision.transform.tag == "Player" && rb.velocity.sqrMagnitude > velocityThreshold * velocityThreshold && dmg)
        {
            collision.transform.GetComponent<PlayerBodyFSM>().damagePlayer(damage, lastImpulse, GetComponent<Rigidbody>().velocity, transform.position);
        }
    }
}
