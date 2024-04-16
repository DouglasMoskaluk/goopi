using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class FireworkAudioPlayer : MonoBehaviour
{
    public void OnVFXOutputEvent(VFXEventAttribute eventAttribute)
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.FIREWORK_BOOM);

    }
}
