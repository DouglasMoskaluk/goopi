using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera[] cams;

    private CinemachineBasicMultiChannelPerlin[] perlins;

    private CinemachineFreeLook freelook;

    [SerializeField]
    private bool willShake = true;

    // Start is called before the first frame update
    void Start()
    {
        freelook = transform.GetChild(2).GetComponent<CinemachineFreeLook>();
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

}
