using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownRagdoll : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 10.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    public void InitializeRagdoll(Vector3 gunVelocity)
    {
        rb.velocity = gunVelocity;
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
}
