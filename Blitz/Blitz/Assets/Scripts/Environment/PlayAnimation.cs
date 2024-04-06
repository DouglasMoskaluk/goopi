using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particles;
    [SerializeField]
    private float timer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            particles.Play();
        }
    }

}
