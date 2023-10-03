using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Vectors
    public Vector2 motionInput { get; private set; }
    public Vector2 lookInput { get; private set; }
    #endregion

    #region bools

    private PlayerInputActions input;

    public bool jumpPressed { get; private set; } = false;
    public bool shootPressed { get; private set; } = false;
    private bool toggleSprint;
    public bool sprintActive { get; private set; } = false;

    public bool reloadPressed { get; private set; } = false;
    public bool dropGrenadePressed { get; private set;} = false;
    public bool throwGrenadePressed { get; private set; } = false;
    public bool optionsPressed { get; private set; } = false;
    #endregion

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void Update()
    {
        //Debug.Log(motionInput);
    }

    public void GetLookInput(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>().normalized;
    }

    public void GetMotionInput(InputAction.CallbackContext ctx)
    {
        motionInput = ctx.ReadValue<Vector2>().normalized;
    }

    public void GetButtonInput(InputAction.CallbackContext ctx)//replace with individual events
    {

    }


}
