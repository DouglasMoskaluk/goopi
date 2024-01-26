using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPiston : MonoBehaviour
{

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

    //private float ratio = 0f;

    private float fallPercentage = 0f;

    private bool inFall = false;

    private IEnumerator raiseCoRo;

    // Start is called before the first frame update
    private void Start()
    {
        raiseCoRo = PistonRaise();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine(PistonFall());
        }
    }

    IEnumerator PistonFall()
    {
        if(!inFall)
        {
            StopCoroutine(raiseCoRo);
            inFall = true;
            float fallTracker = 0;
            float ratio = fallPercentage + (fallTracker / fallTime);

            while (ratio < 1.0f)
            {
                fallTracker += Time.deltaTime;
                PistonObject.transform.position = Vector3.Lerp(StartPoint, EndPoint, ratio);
            }

            //play sound effect
            //play vfx
            //shake camera

            //wait fraction of second
            fallPercentage = 1;
            yield return new WaitForSeconds(0.5f);
            inFall = false;
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
        float raiseTracker = raiseTime;
        fallPercentage = raiseTracker / fallTime;
        while(fallPercentage > 0)
        {
            raiseTracker -= Time.deltaTime;
            PistonObject.transform.position = Vector3.Lerp(StartPoint, EndPoint, fallPercentage);
        }


        yield return null;
    }

}
