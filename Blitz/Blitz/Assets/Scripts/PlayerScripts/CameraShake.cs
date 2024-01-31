using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera[] cams;

    private CinemachineBasicMultiChannelPerlin[] perlins;

    private CinemachineFreeLook freelook;

    private PlayerCamInput camInput;

    private PlayerBodyFSM player;

    private SensitivityHandler senseChanger;

    [SerializeField]
    private bool willShake = true;

    // Start is called before the first frame update
    void Start()
    {
        freelook = transform.GetChild(2).GetComponent<CinemachineFreeLook>();
        camInput = transform.GetChild(2).GetComponent<PlayerCamInput>();
        player = transform.GetComponent<PlayerBodyFSM>();
        senseChanger = transform.GetComponent<SensitivityHandler>();
        senseChanger.enabled = false;
        cams = new CinemachineVirtualCamera[3];
        perlins = new CinemachineBasicMultiChannelPerlin[3];

        for(int i = 0; i < cams.Length; i++)
        {
            cams[i] = freelook.GetRig(i);
            perlins[i] = cams[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

    }

    public void ShakeCamera(float strength, float length)
    {
        StopAllCoroutines();
        StartCoroutine(cameraShake(strength, length));
    }

    public void CharSelectRotateCamera(float strength)
    {
        StartCoroutine(CharCameraRotate(strength));
    }

    IEnumerator cameraShake(float strength, float length)
    {

        if(willShake)
        {
            float timetracker = 0;

            for (int i = 0; i < cams.Length; i++)
            {
                perlins[i].m_AmplitudeGain = strength;
            }

            while (timetracker <= length)
            {
                timetracker += Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < cams.Length; i++)
            {
                perlins[i].m_AmplitudeGain = 0;
            }
        }



        yield return null;
    }

    IEnumerator CharCameraRotate(float strength)
    {

        while(freelook.m_XAxis.Value >= -90)
        {
            freelook.m_XAxis.Value -= strength * Time.deltaTime;
            yield return null;
        }

        freelook.m_XAxis.Value = -90;

        camInput.charSelect = 1;

        player.enabled = true;

        senseChanger.enabled = true;

        yield return null;

    }


}
