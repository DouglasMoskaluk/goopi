using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeThrower : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;

    [SerializeField] private int heldGrenadeCount;

    [SerializeField] private float throwSpeed = 15f;
    [SerializeField] private float coolDownTimer = 1f;

    private bool onCoolDown = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    public bool hasGrenade()
    {
        return heldGrenadeCount > 0 && !onCoolDown;
    }

    public void ThrowGrenade()
    {
        Debug.Log("throw grenade");
        if (heldGrenadeCount < 1 && !onCoolDown) { return; }

        ImpulseGrenade grenade = Instantiate(grenadePrefab, transform.position + Vector3.forward + Vector3.up, Quaternion.identity).GetComponent<ImpulseGrenade>();
        grenade.setDirectionAndSpeed(transform.forward, throwSpeed);
        heldGrenadeCount--;
        onCoolDown = true;
        StartCoroutine(grenadeCD(coolDownTimer));
    }


    public void DropGrenade()
    {
        Debug.Log("drop grenade");
        if (heldGrenadeCount < 1 && !onCoolDown) { return; }

        ImpulseGrenade grenade = Instantiate(grenadePrefab, transform.position + Vector3.forward + Vector3.up, Quaternion.identity).GetComponent<ImpulseGrenade>();
        grenade.setDirectionAndSpeed(-transform.up, 0.5f);
        heldGrenadeCount--;
        onCoolDown = true;
        StartCoroutine(grenadeCD(coolDownTimer));
    }

    private IEnumerator grenadeCD(float time)
    {
        yield return new WaitForSeconds(time);
        onCoolDown = false;
    }
}
