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
        }
    }

    internal void toggleMegaGun(bool enabled)
    {
        StartingMegaGunPickup.SetActive(enabled);
    }
}
