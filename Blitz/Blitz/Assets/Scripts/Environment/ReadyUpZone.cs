using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUpZone : MonoBehaviour
{
    [SerializeField] private int zoneID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.READY_UP);
            LockerRoomManager.instance.ReadyUpPlayer(zoneID);
        }
    }
}
