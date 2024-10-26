using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class RumbleHandler : MonoBehaviour
{

    private Gamepad playerGamepad;

    private IEnumerator shooting;

    private IEnumerator dualEngine;

    private IEnumerator slideCoro;

    private PlayerMotionStates rumbleState;

    private PlayerBodyFSM player;

    private bool endOfSlide = false;

    private void Awake()
    {
        if (GetComponent<PlayerInput>().devices[0] is XInputController or DualShockGamepad)
        {
            playerGamepad = (Gamepad)GetComponent<PlayerInput>().devices[0];
        }
        else
        {
            //Debug.Log("KEYBOARD OR MOUSE");
            playerGamepad = null;
        }



        player = transform.GetComponent<PlayerBodyFSM>();

        EventManager.instance.addListener(Events.onRoundEnd, StopRumble);

        shooting = gunRumble(0, 0);
        dualEngine = DualEngineRumble(0, 0);
        slideCoro = slideRumble();
        rumbleState = PlayerMotionStates.Slide;
        StartCoroutine(slideCoro);
    }

    /// <summary>
    /// to call shoot co routine
    /// </summary>
    public void ShootRumble(int gunType)
    {
        StopCoroutine(shooting);
        //StopAllCoroutines();

        switch(gunType)
        {
            case 1: //nerf
                if (playerGamepad is XInputController)
                {
                    shooting = gunRumble(0.8f, 0.075f);
                    StartCoroutine(shooting);
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    shooting = gunRumble(0.8f, 0.075f);
                    StartCoroutine(shooting);
                }
                break;
            case 2://goop
                if (playerGamepad is XInputController)
                {
                    shooting = gunRumble(0.8f, 0.125f);
                    StartCoroutine(shooting);
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    shooting = gunRumble(0.8f, 0.125f);
                    StartCoroutine(shooting);
                }
                break;
            case 3://ice
                if (playerGamepad is XInputController)
                {
                    shooting = gunRumble(0.6f, 0.1f);
                    StartCoroutine(shooting);
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    shooting = gunRumble(0.6f, 0.1f);
                    StartCoroutine(shooting);
                }
                break;
            case 4://plunger
                if (playerGamepad is XInputController)
                {
                    shooting = gunRumble(0.8f, 0.1f);
                    StartCoroutine(shooting);
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    shooting = gunRumble(0.8f, 0.1f);
                    StartCoroutine(shooting);
                }
                break;
            case 5://fish
                if (playerGamepad is XInputController)
                {
                    shooting = gunRumble(0.55f, 0.3f);
                    StartCoroutine(shooting);
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    shooting = gunRumble(0.55f, 0.3f);
                    StartCoroutine(shooting);
                }
                break;
            case 6://mega
                if (playerGamepad is XInputController)
                {
                    shooting = DualEngineRumble(10, 0.25f);
                    StartCoroutine(shooting);
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    shooting = DualEngineRumble(10, 0.25f);
                    StartCoroutine(shooting);
                }
                break;
        }
    }

    /// <summary>
    /// coroutine to rumble on shoot
    /// </summary>
    IEnumerator gunRumble(float motorValue, float rumbleLength)
    {
        playerGamepad.SetMotorSpeeds(0.0f, motorValue);

        float timerlength = 0;

        while(timerlength <= rumbleLength)
        {
            timerlength += Time.deltaTime;
            yield return null;
        }

        playerGamepad.SetMotorSpeeds(0f, 0f);
    }



    public void DeathRumble()
    {
        //StopAllCoroutines();
        StopCoroutine(shooting);
        StopCoroutine(dualEngine);
        //start death rumble
        dualEngine = DualEngineRumble(6, 2);
        if (playerGamepad != null)
        {
            StartCoroutine(dualEngine);
        }
    }

    public void startDualRumble(float value, float length)
    {
        StopCoroutine(dualEngine);

        if(playerGamepad != null)
        {
            dualEngine = DualEngineRumble(value, length);
            StartCoroutine(dualEngine);
        }
    }

    private IEnumerator slideRumble()
    {
        //StopCoroutine(dualEngine);

        while(true)
        {
            if(player.currentMotionStateFlag == rumbleState)
            {
                if (playerGamepad != null)
                {
                    playerGamepad.SetMotorSpeeds(0.3f, 0.1f);
                    endOfSlide = true;
                    //yield return null;
                    //playerGamepad.SetMotorSpeeds(0,0);
                }
            }
            else if(endOfSlide)
            {
                if (playerGamepad != null)
                {
                    playerGamepad.SetMotorSpeeds(0.0f, 0.0f);
                    endOfSlide = false;
                    //yield return null;
                    //playerGamepad.SetMotorSpeeds(0,0);
                }
            }
            //else
            //{
            //    if (playerGamepad != null)
            //    {
            //        playerGamepad.SetMotorSpeeds(0,0);
            //    }
            //}
            yield return null;
        }
    }

    public void RumbleTick()//only in roulette
    {
        if (playerGamepad != null)
        {
            StartCoroutine(RouletteTick());
        }
    }

    public void StopRumble(EventParams param = new EventParams())
    {
        StopAllCoroutines();
        StopCoroutine(shooting);
        StopCoroutine(dualEngine);

        if (playerGamepad != null)
        {
            playerGamepad.SetMotorSpeeds(0, 0);
        }
    }

   private IEnumerator RouletteTick()
    {
        //float timer = 0f;


        //while(timer < 2f)
        //{
        //    timer += Time.deltaTime;
        //    playerGamepad.SetMotorSpeeds(0.0f, 0.1f);
        //    yield return null;
        //    timer += Time.deltaTime;
        //    playerGamepad.SetMotorSpeeds(0f, 0f);
        //}

        playerGamepad.SetMotorSpeeds(0.0f, 0.25f);

        yield return null;

        playerGamepad.SetMotorSpeeds(0.0f, 0.0f);
    }

    public IEnumerator DualEngineRumble(float divisor, float length)
    {
        playerGamepad.SetMotorSpeeds(1.0f, 1.0f/divisor);

        float timerlength = 0;

        while (timerlength <= length)
        {
            timerlength += Time.deltaTime;
            yield return null;
        }
            playerGamepad.SetMotorSpeeds(0, 0);
    }

}
