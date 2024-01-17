using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleHandler : MonoBehaviour
{
    /// <summary>
    /// to call shoot co routine
    /// </summary>
    public void ShootRumble(int gunType)
    {
        switch(gunType)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }

        StartCoroutine(gunRumble());
    }

    /// <summary>
    /// coroutine to rumble on shoot
    /// </summary>
    IEnumerator gunRumble()
    {
        Gamepad.current.SetMotorSpeeds(0.0f, 0.005f);
        yield return new WaitForSeconds(0.02f);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

}
