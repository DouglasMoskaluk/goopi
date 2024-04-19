using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[RequireComponent(typeof(VisualEffect))]
public class FireworkAudioPlayer : VFXOutputEventAbstractHandler
{
    public override bool canExecuteInEditor => true;

    public override void OnVFXOutputEvent(VFXEventAttribute eventAttribute)
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.FIREWORK_BOOM);
        //Debug.Log("bouttta");
    }
}
