using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    private void Awake()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
    }

    public void SpawnConfettiOnRight()
    {
        particles[1].Play();
    }

    public void SpawnConfettiOnLeft()
    {
        particles[0].Play();
    }

    public void SpawnConfettiBoth()
    {
        SpawnConfettiOnLeft();
        SpawnConfettiOnRight();
    }
}
