using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    #region Vectors
    public bool DisplayDebugMessages = false;
    public Vector2 motionInput { get; private set; }
    public Vector2 lookInput { get; private set; }

    [SerializeField] public Vector2 lookSense;
    #endregion

    #region bools

    private PlayerInputActions input;

    public bool jumpPressed { get; private set; } = false;
    public bool shootPressed { get; private set; } = false;
    public bool toggleSprint { get; private set; } = false;

    public bool reloadPressed { get; private set; } = false;
    public bool dropGrenadePressed { get; private set;} = false;
    public bool throwGrenadePressed { get; private set; } = false;
    public bool optionsPressed { get; private set; } = false;
    #endregion


    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        input = new PlayerInputActions();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getLookInput(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
        lookInput = new Vector2(lookInput.x * lookSense.x, lookInput.y * lookSense.y);
        if (DisplayDebugMessages)
        {
            Debug.Log("look input: " + lookInput);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getMotionInput(InputAction.CallbackContext ctx)
    {
        motionInput = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getReloadInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            reloadPressed = true;
        }
        else if (ctx.canceled)
        {
            reloadPressed = false;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getJumpInput(InputAction.CallbackContext ctx)
    {
       
        if (ctx.started)
        {
            jumpPressed = true;
        }
        else if (ctx.canceled)
        {
            jumpPressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getShootInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            shootPressed = true;
        }
        else if (ctx.canceled)
        {
            shootPressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getSprintInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            toggleSprint = !toggleSprint;
        }
       
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getDropGrenadeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            dropGrenadePressed = true;
        }
        else if (ctx.canceled)
        {
            dropGrenadePressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getThrowGrenadeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            throwGrenadePressed = true;
        }
        else if (ctx.canceled)
        {
            throwGrenadePressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getOptionsInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            optionsPressed = true;
        }
        else if (ctx.canceled)
        {
            optionsPressed = false;
        }
    }
}
