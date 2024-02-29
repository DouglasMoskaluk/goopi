using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    [SerializeField]
    private Gun.GunType gunPickup;
    [SerializeField]
    private float returnHeight = 0;
    private static Vector3 startPos = Vector3.zero;
    [SerializeField]
    GameObject megaGunSkybeam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int id = SplitScreenManager.instance.getPlayerID(other.gameObject);
            GunManager.instance.assignGun(id, (int)gunPickup-1);
            if (gunPickup == Gun.GunType.BOOMSTICK)
            {
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_PICKUP);
                Instantiate(megaGunSkybeam, other.transform.position, other.transform.rotation, other.transform).GetComponent<PlayerAttachedDeletion>().init(id);
            }
            Destroy(transform.parent.gameObject);
        }
    }

    private void Start()
    {
        if (startPos == Vector3.zero)
        {
            startPos = transform.parent.position;
        } else if (transform.parent.position.y < returnHeight || !DomeSlayer.instance.inDome(transform))
        {
            transform.parent.position = startPos;
        }
    }
}
