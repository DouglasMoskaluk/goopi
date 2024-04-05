using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class confettiRepeatBurst : MonoBehaviour
{
    private ParticleSystem particle;
    [SerializeField] private float delay = 0;
    [SerializeField] private float offset = 0;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        StartCoroutine(Burst());
    }

    private IEnumerator Burst()
    {
        while (true) {
            yield return new WaitForSecondsRealtime(delay + offset);
            particle.Play();
        }

    }
}
