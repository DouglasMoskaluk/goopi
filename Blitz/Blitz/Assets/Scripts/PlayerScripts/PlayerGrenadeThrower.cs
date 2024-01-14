using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerGrenadeThrower : MonoBehaviour
{
    
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform grenadeVisualsParent;
    private Transform[] grenadeVisuals;

    [SerializeField] private int heldGrenadeCount;

    [SerializeField] public float throwSpeed = 15f;
    [SerializeField] public float arcAngle = 0f;
    [SerializeField] private float coolDownTimer = 1f;
    [SerializeField, Range(0f, 1f)] private float dropVsThrowThreshold = 0.1f;// below is drop, above is throw
    [SerializeField] private Transform throwFromPoint;

    private bool onCoolDown = false;

    [HideInInspector]
    public int HeldGrenadeCount { get { return heldGrenadeCount; } }
    public int MaxHeldGrenades = 4;

    private void Awake()
    {
        grenadeVisuals = new Transform[MaxHeldGrenades];
        int index = 0;
        foreach (Transform child in grenadeVisualsParent)
        {
            grenadeVisuals[index] = child;
            index++;
        }
    }

    private void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, ResetGrenades);
    }

    public bool hasGrenade()
    {
        return heldGrenadeCount > 0 && !onCoolDown;
    }

    public void ThrowGrenade(Vector3 dir, float chargeModifier)
    {
        Debug.Log("throw grenade");
        if (heldGrenadeCount < 1 && !onCoolDown) { return; }

        ImpulseGrenade grenade = Instantiate(grenadePrefab, throwFromPoint.position, Quaternion.identity, GunManager.instance.transform).GetComponent<ImpulseGrenade>();
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

        RemoveGrenade(1);
        onCoolDown = true;
        StartCoroutine(grenadeCD(coolDownTimer));
    }

    public void setGrenades(int to)
    {
        heldGrenadeCount = to;
        UpdateGrenadeVisual();
    }

    public void RemoveGrenade(int amount)
    {
        heldGrenadeCount = Mathf.Max(heldGrenadeCount - amount, 0);
        UpdateGrenadeVisual();
    }

    public void addGrenade(int amount)
    {
        heldGrenadeCount = Mathf.Min(heldGrenadeCount + amount, MaxHeldGrenades);
        UpdateGrenadeVisual();
    }

    private void UpdateGrenadeVisual()
    {
        for (int i = 0; i < grenadeVisuals.Length; i++)
        {
            if (i < heldGrenadeCount) { grenadeVisuals[i].gameObject.SetActive(true); }
            else { grenadeVisuals[i].gameObject.SetActive(false); }
        }
    }

    public void ResetGrenades(EventParams param = new EventParams())
    {
        setGrenades(MaxHeldGrenades);
    }

    public void DropGrenade(Vector3 dir)
    {
        Debug.Log("drop grenade");
        if (heldGrenadeCount < 1 && !onCoolDown) { return; }

        ImpulseGrenade grenade = Instantiate(grenadePrefab, transform.position + dir.normalized + Vector3.up, Quaternion.identity).GetComponent<ImpulseGrenade>();
        grenade.init(GetComponent<PlayerBodyFSM>().playerID);
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
