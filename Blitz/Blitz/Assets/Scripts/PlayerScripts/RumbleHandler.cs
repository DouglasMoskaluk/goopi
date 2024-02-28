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
            case 0:
                if(playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.3f, 0.1f));
                }
                else if(playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 1:
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.3f, 0.1f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 2:
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.3f, 0.1f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 3:
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.3f, 0.1f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 4:
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.3f, 0.1f));
                }
                else if (playerGamepad is DualShockGamepad)
                {
                    StartCoroutine(gunRumble(0.005f, 0.02f));
                }
                break;
            case 5:
                if (playerGamepad is XInputController)
                {
                    StartCoroutine(gunRumble(0.3f, 0.1f));
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

    public void startDualRumble(float value, float length)
    {
        if(playerGamepad != null)
        {
            StartCoroutine(DualEngineRumble(value, length));
        }
    }

    public IEnumerator DualEngineRumble(float value, float length)
    {
            playerGamepad.SetMotorSpeeds(value, value/4);

        float timerlength = 0;

        while (timerlength <= length)
        {
            timerlength += Time.deltaTime;
            yield return null;
        }
            playerGamepad.SetMotorSpeeds(0, 0);
    }

}
