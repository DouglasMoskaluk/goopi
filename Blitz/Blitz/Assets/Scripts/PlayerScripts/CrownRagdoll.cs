using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownRagdoll : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 10.0f;

    private Rigidbody rb;

    void Awake()
    {
        rb = transform.GetChild(0).GetComponent<Rigidbody>();
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    public void InitializeRagdoll(Vector3 playerVelocity)
    {
        rb.velocity += playerVelocity;
    }

    public void DeathForce(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity += direction * 4f;
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
