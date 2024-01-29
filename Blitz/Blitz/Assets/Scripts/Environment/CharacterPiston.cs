using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPiston : MonoBehaviour
{

    public float shakeStrength;

    public float shakeLength;

    [SerializeField]
    private GameObject PistonObject;

    [SerializeField]
    private Vector3 StartPoint;

    [SerializeField]
    private Vector3 EndPoint;

    [SerializeField]
    private float fallTime = 0.5f;

    [SerializeField]
    private float raiseTime = 4f;

    [SerializeField]
    private float waitTime = 0.25f;

    //private float ratio = 0f;

    private float fallPercentage = 0f;

    private bool inFall = false;

    private IEnumerator raiseCoRo;

    private IEnumerator fallCoro;

    
    public CameraShake playerCamShake;

    // Start is called before the first frame update
    private void Start()
    {
        PistonObject.transform.position = StartPoint;
        raiseCoRo = PistonRaise();
        fallCoro = PistonFall();
    }

    public void getPlayerCameraShake(CameraShake newPlayerCamShake)
    {
        playerCamShake = newPlayerCamShake;
    }

    public void LowerPiston()
    {
       StopCoroutine(fallCoro);
        inFall = false;

        fallCoro = PistonFall();    

       StartCoroutine(fallCoro);
    }

    IEnumerator PistonFall()
    {
        if(!inFall)
        {
            inFall = true;
            //Debug.Log("FALL");
            StopCoroutine(raiseCoRo);

            float fallTracker = 0;
            float ratio = fallPercentage;

            while (ratio < 1.0f)
            {
                fallTracker += Time.deltaTime;
                //Debug.Log(fallTracker);
                ratio = fallPercentage + (fallTracker / fallTime);
                fallPercentage = ratio;
                PistonObject.transform.position = Vector3.Lerp(StartPoint, EndPoint, ratio);
                //Debug.Log(ratio);
                yield return null;
            }

            PistonObject.transform.position = Vector3.Lerp(StartPoint, EndPoint, 1);


            //play sound effect
            //play vfx
            //shake camera

            //wait fraction of second
            fallPercentage = 1;

            playerCamShake.ShakeCamera(shakeStrength, shakeLength);

            //yield return new WaitForSeconds(waitTime);
            inFall = false;
            raiseCoRo = PistonRaise();
            StartCoroutine(raiseCoRo);
        }
        yield return null;

        //raise

        //while (ratio > 0.0f)
        //{
        //    fallTracker -= Time.deltaTime;
        //    PistonObject.transform.position = Vector3.Lerp(StartPoint, EndPoint, ratio);
        //}

        //yield return null;

    }

    IEnumerator PistonRaise()
    {
        Debug.Log("RAISE");
        float raiseTracker = raiseTime;
        fallPercentage = raiseTracker / raiseTime;
        Debug.Log(fallPercentage);
        while(fallPercentage >= 0)
        {
            raiseTracker -= Time.deltaTime;
            Debug.Log(raiseTracker);
            fallPercentage = raiseTracker / raiseTime;
            PistonObject.transform.position = Vector3.Lerp(StartPoint, EndPoint, fallPercentage);
            yield return null;
        }

        PistonObject.transform.position = Vector3.Lerp(StartPoint, EndPoint, 0);



        yield return null;
    }

}
