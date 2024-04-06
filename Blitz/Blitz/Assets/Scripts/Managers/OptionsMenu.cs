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
    [SerializeField]
    internal Toggle fullscreenToggle;

    public void setVolume(int group)
    {
        mainMixer.SetFloat(mixerNames[group], Mathf.Log(volumeSliders[group].value) * 20);
    }

    public void windowed()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        if (fullscreenToggle.isOn)
        {
            FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.fullScreenMode = fullScreenMode;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, fullScreenMode);
        }
        else if (!fullscreenToggle.isOn)
        {
            Screen.SetResolution(192 * 4, 108 * 4, false);

        }

    }

    private void OnEnable()
    {
        GetComponent<Animator>().SetBool("SM", true);
    }


    public void DisableSettings()
    {
        gameObject.SetActive(false);
    }


    internal void loadSettings()
    {
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            float val;
            mainMixer.GetFloat(mixerNames[i], out val);
            volumeSliders[i].SetValueWithoutNotify(Mathf.Pow(10, val / 20f));
        }

        if (Screen.fullScreen) fullscreenToggle.isOn = true;
        else fullscreenToggle.isOn = false;
    }
}
