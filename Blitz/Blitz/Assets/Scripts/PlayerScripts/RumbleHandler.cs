using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class RumbleHandler : MonoBehaviour
{

    private Gamepad playerGamepad;

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
    }

    /// <summary>
    /// to call shoot co routine
    /// </summary>
    public void ShootRumble(int gunType)
    {
        StopAllCoroutines();

        switch(gunType)
        {
            case 1: //nerf
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.6f, 0.05f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 2://goop
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.6f, 0.1f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 3://ice
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.4f, 0.075f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 4://plunger
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.8f, 0.05f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 5://fish
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.35f, 0.3f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 6://mega
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(DualEngineRumble(8, 0.15f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
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
