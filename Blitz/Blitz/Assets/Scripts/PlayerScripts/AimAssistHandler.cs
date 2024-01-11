using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistHandler : MonoBehaviour
{
    [SerializeField]
    private Transform castPoint;

    [SerializeField]
    private float sphereSize;

    [SerializeField]
    private CinemachineFreeLook freeLook;

    [SerializeField]
    private SensitivityHandler sensitivityHandler;

    // Start is called before the first frame update
    void Awake()
    {
    //    castPoint = GetComponentInChildren<Transform>();
    //freeLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.SphereCast(castPoint.position, sphereSize, castPoint.forward, out hit, 30));
        {
            if(hit.collider != null)
            {
                if (hit.transform.CompareTag("Player") && hit.distance > 4)
                {
                    Debug.Log("ON PLAYER");
                    //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity * 0.8f;
                    //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity * 0.8f;
                    //lower sensitivity
                    //get position of playerbody
                    //rotate cinemachine camera by X based on distance

                }
                else
                {
                    //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity;
                    //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity;
                    //sensitivity is normal
                }
            }
        }
    }
}
