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

    private IEnumerator sliding;

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

        shooting = gunRumble(0, 0);

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

    public void deathRumble()
    {
        
    }

    public void startDualRumble(float value, float length)
    {
        if(playerGamepad != null)
        {
            StartCoroutine(DualEngineRumble(value, length));
        }
    }

    public void slideRumble(bool isActive)
    {
        if(playerGamepad != null)
        {
            StartCoroutine(slideRumble());
        }
    }

    public void RumbleTick()
    {
        if (playerGamepad != null)
        {
            StartCoroutine(RouletteTick());
        }
    }

   private IEnumerator RouletteTick()
    {
        playerGamepad.SetMotorSpeeds(0.0f, 0.3f);

        yield return new WaitForSecondsRealtime(0.005f);

        playerGamepad.SetMotorSpeeds(0.0f, 0.0f);
    }

    public IEnumerator slideRumble()
    {
        yield return null;
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
