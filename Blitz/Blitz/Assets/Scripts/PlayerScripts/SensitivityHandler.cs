using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SensitivityHandler : MonoBehaviour
{
    SensitivityHandler sensitivityHandler;

    private CinemachineFreeLook freeLook;

    private PlayerInputHandler inputValues;

    public float XSensitivity = 300;
    public float YSensitivity = 2;

    // Start is called before the first frame update
    void Start()
    {
        sensitivityHandler = this;
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
            XSensitivity = Mathf.Clamp(XSensitivity + (inputValues.sensitivityInput.x*50), 100, 800);
            YSensitivity = Mathf.Clamp(YSensitivity + (inputValues.sensitivityInput.y * 0.5f), 1, 6);
            freeLook.m_XAxis.m_MaxSpeed = XSensitivity;
            freeLook.m_YAxis.m_MaxSpeed = YSensitivity;
        }
    }

}
