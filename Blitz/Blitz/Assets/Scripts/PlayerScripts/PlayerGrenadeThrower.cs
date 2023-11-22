using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeThrower : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;

    [SerializeField] private int heldGrenadeCount;

    [SerializeField] public float throwSpeed = 15f;
    [SerializeField] public float arcAngle = 0f;
    [SerializeField] private float coolDownTimer = 1f;
    [SerializeField, Range(0f, 1f)] private float dropVsThrowThreshold = 0.1f;// below is drop, above is throw
    [SerializeField] private Transform throwFromPoint;

    private bool onCoolDown = false;

    [HideInInspector]
    public int HeldGrenadeCount { get { return heldGrenadeCount; } }
    

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

    public void ThrowGrenade(Vector3 dir, float chargeModifier)
    {
        Debug.Log("throw grenade");
        if (heldGrenadeCount < 1 && !onCoolDown) { return; }

        ImpulseGrenade grenade = Instantiate(grenadePrefab, throwFromPoint.position, Quaternion.identity).GetComponent<ImpulseGrenade>();
        grenade.setDirectionAndSpeed(dir, throwSpeed * chargeModifier);
        if (chargeModifier >= dropVsThrowThreshold)
        {
            grenade.setGrenadeType(GrenadeType.Thrown);
            grenade.setDirectionAndSpeed(dir, throwSpeed * chargeModifier);
        }
        else
        {
            grenade.setGrenadeType(GrenadeType.Dropped);
            grenade.setDirectionAndSpeed(Vector3.down, 5f);
        }
        
        heldGrenadeCount--;
        onCoolDown = true;
        StartCoroutine(grenadeCD(coolDownTimer));
    }

    public void setGrenades(int to)
    {
        heldGrenadeCount = to;
    }

    public void addGrenade(int amount)
    {
        heldGrenadeCount += amount;
    }


    public void DropGrenade(Vector3 dir)
    {
        Debug.Log("drop grenade");
        if (heldGrenadeCount < 1 && !onCoolDown) { return; }

        ImpulseGrenade grenade = Instantiate(grenadePrefab, transform.position + dir.normalized + Vector3.up, Quaternion.identity).GetComponent<ImpulseGrenade>();
        grenade.setDirectionAndSpeed(-transform.up, 3f);
        grenade.setGrenadeType(GrenadeType.Dropped);
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
