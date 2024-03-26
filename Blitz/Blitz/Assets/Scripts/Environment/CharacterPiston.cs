using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class CharacterPiston : MonoBehaviour
{

    public float shakeStrength;

    public float shakeLength;

    [SerializeField]
    private GameObject PistonObject;

    [SerializeField]
    private Transform StartPoint;

    [SerializeField]
    private Transform EndPoint;

    [SerializeField]
    private float fallTime = 0.5f;

    [SerializeField]
    private float raiseTime = 4f;

    [SerializeField]
    private float waitTime = 0.25f;

    [SerializeField]
    private GameObject smokeFX;

    public float rumbleStrength;

    public float rumbleLength;

    //private float ratio = 0f;

    private float fallPercentage = 0f;

    private bool inFall = false;

    private IEnumerator raiseCoRo;

    private IEnumerator fallCoro;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public PlayerBodyFSM playerFSM;

    [HideInInspector]
    public CameraShake playerCamShake;

    private PlayerCharSelectAnims playerAnim;

    private RumbleHandler rumble;

    private bool firstEntry = true;

    // Start is called before the first frame update
    private void Start()
    {
        PistonObject.transform.position = StartPoint.position;
        raiseCoRo = PistonRaise();
        fallCoro = PistonFall();
    }

    public void getPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        playerCamShake = player.GetComponent<CameraShake>();
        playerFSM = player.GetComponent<PlayerBodyFSM>();
        rumble = player.GetComponent<RumbleHandler>();
        playerAnim = player.transform.GetChild(1).GetComponent<PlayerCharSelectAnims>();
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
            bool didFall = true;
            //Debug.Log("FALL");
            StopCoroutine(raiseCoRo);

            float fallTracker = 0;
            float ratio = fallPercentage;

            if(ratio >= 1)
            {
                didFall = false;
            }

            while (ratio < 1.0f)
            {
                fallTracker += Time.deltaTime;
                //Debug.Log(fallTracker);
                ratio = fallPercentage + (fallTracker / fallTime);
                fallPercentage = ratio;
                PistonObject.transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, ratio);
                //Debug.Log(ratio);
                yield return null;
            }

            PistonObject.transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, 1);


            //play sound effect
            //play vfx
            //shake camera

            //wait fraction of second
            fallPercentage = 1;

            if(didFall)
            {
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.PRESS_SLAM);

                Instantiate(smokeFX, new Vector3(EndPoint.position.x, EndPoint.position.y - 2, EndPoint.position.z), Quaternion.identity);

                playerCamShake.ShakeCamera(shakeStrength, shakeLength);
                rumble.startDualRumble(shakeStrength, shakeLength);
            }

            yield return new WaitForSecondsRealtime(waitTime);

            //playerCamShake.ShakeCamera(shakeStrength, shakeLength);

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
        //Debug.Log("RAISE");
        float raiseTracker = raiseTime;
        fallPercentage = raiseTracker / raiseTime;
        //Debug.Log(fallPercentage);
        playerAnim.UnSquash();
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.PRESS_HISS);
        while (fallPercentage >= 0)
        {
            raiseTracker -= Time.deltaTime;
            //Debug.Log(raiseTracker);
            fallPercentage = raiseTracker / raiseTime;
            PistonObject.transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, fallPercentage);
            yield return null;
        }

        PistonObject.transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, 0);



        yield return null;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(firstEntry)
            {
                firstEntry = false;
                LowerPiston();
            }
            else
            {
                StartCoroutine(CharReEntry());
            }
        }
    }

    IEnumerator CharReEntry()
    {
        //maybe UI click sound when entering

        //turn off HUD
        
        //turn off playerFSM
        
        //turn off camera control

        //same loop
        //move player to center of piston
        //rotate camera to face player

        //turn on char select UI


        yield return null;
    }

    IEnumerator CameraRotate()
    {
        yield return null;
    }


}
