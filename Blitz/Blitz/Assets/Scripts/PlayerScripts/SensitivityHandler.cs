using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SensitivityHandler : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    private PlayerInputHandler inputValues;
    // Start is called before the first frame update
    void Start()
    {
        freeLook = GetComponentInChildren<CinemachineFreeLook>();
        inputValues = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Debug.Log("SENSITITVY VALUE: " + inputValues.SensitivityInput);
    //}

    public void ChangeSensitivity()
    {
        if(inputValues.sensitivityInput != new Vector2(0,0))
        {
            //Debug.Log("INPUT VALUES: " +  inputValues.sensitivityInput);
            float newXSpeed = Mathf.Clamp(freeLook.m_XAxis.m_MaxSpeed + (inputValues.sensitivityInput.x*100), 100, 800);
            float newYSpeed = Mathf.Clamp(freeLook.m_YAxis.m_MaxSpeed + (inputValues.sensitivityInput.y * 0.5f), 1, 6);
            freeLook.m_XAxis.m_MaxSpeed = newXSpeed;
            freeLook.m_YAxis.m_MaxSpeed = newYSpeed;
        }
    }

}
