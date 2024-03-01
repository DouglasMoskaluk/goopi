using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SmokeEffectSetUp : MonoBehaviour
{
    private VisualEffect smoke;

    private void Awake()
    {
        smoke = GetComponent<VisualEffect>();
    }

    public void setSmokeAmountAndSize(int amount, float size)
    {
        smoke.SetFloat("Size", size);
        smoke.SetInt("Amount", amount);
    }
}
