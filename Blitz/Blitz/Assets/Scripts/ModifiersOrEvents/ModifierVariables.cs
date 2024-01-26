using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierVariables : MonoBehaviour
{

    [SerializeField]
    GameObject[] BonusPlatformsLavaEvent;
    [SerializeField]
    GameObject StartingMegaGunPickup;

    private void Start()
    {
        ModifierManager.instance.vars = this;
    }

    internal void toggleLava(bool enabled)
    {
        foreach (GameObject go in BonusPlatformsLavaEvent)
        {
            go.SetActive(enabled);
            if (go.transform.childCount > 0 && enabled)
            {
                PingPong ping = go.transform.GetChild(0).GetComponent<PingPong>();
                if (ping != null) ping.resetCoroutine();
            }
        }
    }

    internal void toggleMegaGun(bool enabled)
    {
        StartingMegaGunPickup.SetActive(enabled);
    }
}
