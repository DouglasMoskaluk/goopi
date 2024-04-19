using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu3DanimationHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject impulseFX;

    [SerializeField]
    private Transform impulseSpawnPoint;

    void Start()
    {
        
    }

    public void Explosion()
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.IMPULSE_DETONATE);
        Instantiate(impulseFX, impulseSpawnPoint.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
