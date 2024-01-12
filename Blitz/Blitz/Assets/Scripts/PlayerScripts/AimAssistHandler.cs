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

    private Transform playerTransform;

    [SerializeField]
    private PlayerInputHandler input;

    [SerializeField]
    private PlayerCamInput camInput;

    [SerializeField]
    private float aimSensitivity = 0.75f;

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = transform.parent;
    //    castPoint = GetComponentInChildren<Transform>();
    //freeLook = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        StartCoroutine("AimAssist");
    }

    /// <summary>
    /// Coroutine to run update running aim assist
    /// </summary>
    IEnumerator AimAssist()
    {
        while (true)
        {

            RaycastHit[] hits;

            hits = Physics.SphereCastAll(castPoint.position, sphereSize, castPoint.forward, 30.0f);

            for(int i = 0; i < hits.Length; i++)
            {

                RaycastHit hit = hits[i];

                if (hit.collider != null && (input.motionInput != Vector2.zero || input.lookInput != Vector2.zero))
                {
                    //if(input.motionInput == Vector2.zero && input.lookInput == Vector2.zero)
                    //{
                    //    Debug.Log("NOT MOVING");
                    //}

                    //Debug.Log(input.motionInput);


                    if (hit.transform.CompareTag("Player") && hit.distance > 4)
                    {
                        

                        Transform enemyPos = hit.transform.GetChild(3).transform;

                        RaycastHit lineHit;

                        if (Physics.Linecast(castPoint.position, enemyPos.position, out lineHit) && lineHit.transform.CompareTag("Player"))
                        {

                            camInput.aimAssistSlowdown = aimSensitivity;

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


                            //if (Physics.Linecast(castPoint.position, hit.transform.position)
                            //{

                            //}



                        //Transform enemyPos = hit.transform.GetChild(3).transform;
                        //Debug.Log(hit.point + "   enemy point:" + enemyPos.position);
                        Debug.Log(transform.InverseTransformPoint(hit.point) + "   enemy point:" + transform.InverseTransformPoint(enemyPos.position));
                        //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity * 0.8f;
                        //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity * 0.8f;
                        //lower sensitivity

                        //freeLook.m_XAxis.

                        //get position of playerbody
                        //rotate cinemachine camera by X based on distance
                        //greater turn left




                    }
                    else
                    {
                        camInput.aimAssistSlowdown = 1f;
                    }
                }
            }




            //if (Physics.SphereCast(castPoint.position, sphereSize, castPoint.forward, out hit, 30)) ;
            //{
            //    if (hit.collider != null && (input.motionInput != Vector2.zero || input.lookInput != Vector2.zero))
            //    {   
            //        //if(input.motionInput == Vector2.zero && input.lookInput == Vector2.zero)
            //        //{
            //        //    Debug.Log("NOT MOVING");
            //        //}

            //        //Debug.Log(input.motionInput);
            //        if (hit.transform.CompareTag("Player") && hit.distance > 4)
            //        {

            //            camInput.aimAssistSlowdown = aimSensitivity;


            //            Transform enemyPos = hit.transform.GetChild(3).transform;
            //            //Debug.Log(hit.point + "   enemy point:" + enemyPos.position);
            //            Debug.Log(transform.InverseTransformPoint(hit.point) + "   enemy point:" + transform.InverseTransformPoint(enemyPos.position));
            //            //freeLook.m_XAxis.m_MaxSpeed = sensitivityHandler.XSensitivity * 0.8f;
            //            //freeLook.m_YAxis.m_MaxSpeed = sensitivityHandler.YSensitivity * 0.8f;
            //            //lower sensitivity

            //            //freeLook.m_XAxis.

            //            //get position of playerbody
            //            //rotate cinemachine camera by X based on distance
            //            //greater turn left

            //            if (transform.InverseTransformPoint(hit.point).x > transform.InverseTransformPoint(enemyPos.position).x)
            //            {
            //                float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);
            //                if (absolute < 0.05f)
            //                {
            //                    freeLook.m_XAxis.Value -= 0.025f;
            //                }
            //                else
            //                {
            //                    freeLook.m_XAxis.Value -= 0.35f;
            //                }
            //            }
            //            //less than turn right
            //            else if (transform.InverseTransformPoint(hit.point).x < transform.InverseTransformPoint(enemyPos.position).x)
            //            {
            //                float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);

            //                if (absolute < 0.05f)
            //                {
            //                    freeLook.m_XAxis.Value += 0.025f;
            //                }
            //                else
            //                {
            //                    freeLook.m_XAxis.Value += 0.35f;
            //                }
            //            }


            //        }
            //        else
            //        {
            //            camInput.aimAssistSlowdown = 1f;
            //        }
            //    }
            //}

            yield return new WaitForSeconds(0.005f);
        }

    }

}
