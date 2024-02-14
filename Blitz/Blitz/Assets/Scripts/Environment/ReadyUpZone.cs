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
            LockerRoomManager.instance.ReadyUpPlayer(zoneID);
        }
    }
}
