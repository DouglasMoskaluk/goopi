using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    
    [SerializeField]
    internal string[] mixerNames;
    [SerializeField]
    internal Slider[] volumeSliders;
    [SerializeField]
    internal AudioMixer mainMixer;

    public void setVolume(int group)
    {
        mainMixer.SetFloat(mixerNames[group], Mathf.Log(volumeSliders[group].value) * 20);
    }
}
