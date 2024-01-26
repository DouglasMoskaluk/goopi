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

    [SerializeField]
    private float startDistance = 2f;

    [SerializeField]
    private float endDistance = 30;

    [SerializeField]
    private float stickyness = 0.35f;

    [SerializeField]
    private float deadzone = 0.25f;

    private Transform lastFramePoint;

    private Vector3 lastframePos;

    private bool enterRaycast= false;

    enum AAType {VersionOne, VersionTwo }

    [SerializeField]
    private AAType aimAssistVersion;

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

            hits = Physics.SphereCastAll(castPoint.position, sphereSize, castPoint.forward, endDistance);

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


                    if (hit.transform.CompareTag("Player") && hit.distance > startDistance && hit.transform.gameObject != this.transform.root.gameObject)
                    {
                        //get point of nearest player
                        Transform enemyPos = hit.transform.GetChild(3).transform;

                        RaycastHit lineHit;

                        //if player is visible (not behind cover) start AA
                        if (Physics.Linecast(castPoint.position, enemyPos.position, out lineHit) && lineHit.transform.CompareTag("Player"))
                        {
                            Debug.Log("HIT PLAYER");
                            camInput.aimAssistSlowdown = aimSensitivity;

                            //VERSION 1

                            if(aimAssistVersion == AAType.VersionOne)
                            {
                                if (transform.InverseTransformPoint(hit.point).x > transform.InverseTransformPoint(enemyPos.position).x)
                                {
                                    float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);
                                    if (absolute > deadzone)
                                    {
                                        freeLook.m_XAxis.Value -= stickyness * Time.deltaTime;
                                    }
                                    //else
                                    //{
                                    //    freeLook.m_XAxis.Value -= stickyness;
                                    //}
                                }
                                //less than turn right
                                else if (transform.InverseTransformPoint(hit.point).x < transform.InverseTransformPoint(enemyPos.position).x)
                                {
                                    float absolute = Mathf.Abs(transform.InverseTransformPoint(hit.point).x - transform.InverseTransformPoint(enemyPos.position).x);

                                    if (absolute > deadzone)
                                    {
                                        freeLook.m_XAxis.Value += stickyness * Time.deltaTime;
                                    }
                                    //else
                                    //{
                                    //    freeLook.m_XAxis.Value += stickyness;
                                    //}
                                }
                                break;
                            }

                            //VERSION TWO

                            else if(aimAssistVersion == AAType.VersionTwo)
                            {
                                if(enterRaycast == true) //first frame in AA - set up reference point for next frame
                                {
                                    //lastFramePoint = enemyPos.position;
                                    enterRaycast = false;
                                    lastFramePoint = enemyPos;
                                    lastframePos = transform.InverseTransformPoint(lastFramePoint.position);
                                }
                                else
                                {
                                    //now we gotta do math
                                    //Debug.Log("OLD POINT: " + lastframePos + " NEW POINT: " + transform.InverseTransformPoint(enemyPos.position));
                                    Vector3 oldDirection = lastframePos; // - castPoint.position;
                                    Vector3 newDirection = transform.InverseTransformPoint(enemyPos.position); // - castPoint.position;
                                    float angle = Vector3.Angle(newDirection, oldDirection);

                                    //test

                                    Vector2 old2D = new Vector2(lastframePos.x, lastframePos.z); //- new Vector2(castPoint.position.x, castPoint.position.z);//   lastframePos - castPoint.position;
                                    Vector2 new2D = new Vector2(newDirection.x, newDirection.z);
                                    Debug.Log("OLD POINT: " + old2D + " NEW POINT: " + new2D);
                                    float angle2D = Vector2.Angle(old2D, new2D);

                                    Debug.Log(angle2D);

                                    if (new2D.x < old2D.x)
                                    {
                                        freeLook.m_XAxis.Value -= angle2D * 0.9f;
                                    }
                                    else if (new2D.x > old2D.x)
                                    {
                                        freeLook.m_XAxis.Value += angle2D * 0.9f;
                                    }

                                    lastFramePoint = enemyPos;
                                    lastframePos = transform.InverseTransformPoint(lastFramePoint.position);


                                }
                            }

                        }
                        else //player isnt visible AA turns off and stuff reset
                        {
                            Debug.Log("NOT ON TAREGT");
                            camInput.aimAssistSlowdown = 1f;
                            enterRaycast = false;
                        }

                    }
                    else //no player in raycast so no AA - reset stuff
                    {
                        Debug.Log("NOT ON TAREGT2");

                        camInput.aimAssistSlowdown = 1f;
                        enterRaycast = false;
                    }
                }
            }


            yield return null;
        }

    }

}
