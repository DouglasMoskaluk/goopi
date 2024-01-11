using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Transform playerTransform;

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = transform.parent;
    //    castPoint = GetComponentInChildren<Transform>();
    //freeLook = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        StartCoroutine(AimAssist());
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    RaycastHit hit;

    //    if (Physics.SphereCast(castPoint.position, sphereSize, castPoint.forward, out hit, 30));
    //    {
    //        if(hit.collider != null)
    //        {
    //            if (hit.transform.CompareTag("Player") && hit.distance > 4)
    //            {
    //                Transform enemyPos = hit.transform.GetChild(3).transform;
    //                //Debug.Log(hit.point + "   enemy point:" + enemyPos.position);
    //                Debug.Log(transform.InverseTransformPoint(hit.point) + "   enemy point:" + transform.InverseTransformPoint(enemyPos.position));
    //                //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity * 0.8f;
    //                //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity * 0.8f;
    //                //lower sensitivity
    //                //get position of playerbody
    //                //rotate cinemachine camera by X based on distance
    //                //greater turn left

    //                if(transform.InverseTransformPoint(hit.point).x > transform.InverseTransformPoint(enemyPos.position).x)
    //                {
    //                    float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);
    //                    if (absolute < 0.05f)
    //                    {
    //                        freeLook.m_XAxis.Value -= 0.05f;
    //                    }
    //                    else 
    //                    {
    //                        freeLook.m_XAxis.Value -= 0.625f;
    //                    }
    //                }
    //                //less than turn right
    //                else if (transform.InverseTransformPoint(hit.point).x < transform.InverseTransformPoint(enemyPos.position).x)
    //                { 
    //                    float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);

    //                    if (absolute < 0.05f)
    //                    {
    //                        freeLook.m_XAxis.Value += 0.05f;
    //                    }
    //                    else
    //                    {
    //                        freeLook.m_XAxis.Value += 0.625f;
    //                    }
    //                }


    //            }
    //            else
    //            {
    //                //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity;
    //                //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity;
    //                //sensitivity is normal
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// Coroutine to run update running aim assist
    /// </summary>
    IEnumerator AimAssist()
    {
        while (true)
        {

            RaycastHit hit;

            if (Physics.SphereCast(castPoint.position, sphereSize, castPoint.forward, out hit, 30)) ;
            {
                if (hit.collider != null)
                {
                    if (hit.transform.CompareTag("Player") && hit.distance > 4)
                    {
                        Transform enemyPos = hit.transform.GetChild(3).transform;
                        //Debug.Log(hit.point + "   enemy point:" + enemyPos.position);
                        Debug.Log(transform.InverseTransformPoint(hit.point) + "   enemy point:" + transform.InverseTransformPoint(enemyPos.position));
                        //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity * 0.8f;
                        //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity * 0.8f;
                        //lower sensitivity
                        //get position of playerbody
                        //rotate cinemachine camera by X based on distance
                        //greater turn left

                        if (transform.InverseTransformPoint(hit.point).x > transform.InverseTransformPoint(enemyPos.position).x)
                        {
                            float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);
                            if (absolute < 0.05f)
                            {
                                freeLook.m_XAxis.Value -= 0.025f;
                            }
                            else
                            {
                                freeLook.m_XAxis.Value -= 0.35f;
                            }
                        }
                        //less than turn right
                        else if (transform.InverseTransformPoint(hit.point).x < transform.InverseTransformPoint(enemyPos.position).x)
                        {
                            float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);

                            if (absolute < 0.05f)
                            {
                                freeLook.m_XAxis.Value += 0.025f;
                            }
                            else
                            {
                                freeLook.m_XAxis.Value += 0.35f;
                            }
                        }


                    }
                    else
                    {
                        //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity;
                        //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity;
                        //sensitivity is normal
                    }
                }
            }

            yield return new WaitForSeconds(0.005f);
        }

    }

}
