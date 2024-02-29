using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UIElements;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera[] cams;

    private CinemachineBasicMultiChannelPerlin[] perlins;

    [HideInInspector]
    public CinemachineFreeLook freelook;

    [HideInInspector]
    public PlayerCamInput camInput;

    [HideInInspector]
    public CinemachineCollider camCollider;

    [HideInInspector]
    public CinemachineCameraOffset offset;

    private PlayerBodyFSM player;

    private SensitivityHandler senseChanger;

    [SerializeField]
    private bool willShake = true;

    IEnumerator shakeCoRo;

    // Start is called before the first frame update
    void Start()
    {
        freelook = transform.GetChild(2).GetComponent<CinemachineFreeLook>();
        offset = transform.GetChild(2).GetComponent<CinemachineCameraOffset>();
        camInput = transform.GetChild(2).GetComponent<PlayerCamInput>();
        camCollider = transform.GetChild(2).GetComponent<CinemachineCollider>();
        player = transform.GetComponent<PlayerBodyFSM>();
        senseChanger = transform.GetComponent<SensitivityHandler>();
        senseChanger.enabled = false;
        offset.m_Offset = new Vector3(0.5f, 0.3f, 0);
        cams = new CinemachineVirtualCamera[3];
        perlins = new CinemachineBasicMultiChannelPerlin[3];

        shakeCoRo = cameraShake(0,0);

        for(int i = 0; i < cams.Length; i++)
        {
            cams[i] = freelook.GetRig(i);
            perlins[i] = cams[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

    }

    public void GunShake(int gunType)
    {
        switch (gunType)
        {
            case 1: //nerf
                StopCoroutine(shakeCoRo);

                shakeCoRo = cameraShake(0.5f, 0.1f);

                StartCoroutine(shakeCoRo);
                break;
            case 2://goop
                StopCoroutine(shakeCoRo);

                shakeCoRo = cameraShake(1f, 0.1f);

                StartCoroutine(shakeCoRo);
                break;
            case 3://ice
                StopCoroutine(shakeCoRo);

                shakeCoRo = cameraShake(0.2f, 0.1f);

                StartCoroutine(shakeCoRo);
                break;
            case 4://plunger
                StopCoroutine(shakeCoRo);

                shakeCoRo = cameraShake(0.5f, 0.15f);

                StartCoroutine(shakeCoRo);
                break;
            case 5://fish
                StopCoroutine(shakeCoRo);

                shakeCoRo = cameraShake(0.3f, 0.3f);

                StartCoroutine(shakeCoRo);
                break;
            case 6://mega
                StopCoroutine(shakeCoRo);

                shakeCoRo = cameraShake(4.5f, 0.2f);

                StartCoroutine(shakeCoRo);
                break;
        }
    }

    public void ShakeCamera(float strength, float length)
    {
        //StopAllCoroutines();

        StopCoroutine(shakeCoRo);

        shakeCoRo = cameraShake(strength, length);

        StartCoroutine(shakeCoRo);
    }

    public void ResetOffset()
    {
        offset.m_Offset = new Vector3(0.5f, 0.3f, 0f);
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

        //while(offset.m_Offset.x < 0.5f)
        //{
        //    offset.m_Offset.x += Time.deltaTime * 2.0f;
        //    offset.m_Offset.y += Time.deltaTime * 1.4f;
        //    yield return null;
        //}

        //offset.m_Offset = new Vector3(0.5f, 0.3f, 0);

        camInput.charSelect = 1;

        player.enabled = true;

        senseChanger.enabled = true;

        player.enableHeadMotion();

    }


}
