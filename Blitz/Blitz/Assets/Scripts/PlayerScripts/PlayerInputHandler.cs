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
    public Vector2 motionInput { get; private set; }
    public Vector2 lookInput { get; private set; }

    [SerializeField] public Vector2 lookSense;
    #endregion

    #region bools

    private PlayerInputActions input;

    public bool jumpPressed { get; private set; } = false;
    public bool shootPressed { get; private set; } = false;
    public bool toggleSprint { get; private set; } = false;

    public bool crouchPressed { get; private set; } = false;

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


    private void Update()
    {
        Debug.Log(lookSense);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetLookInput(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>().normalized;
        lookInput = new Vector2(lookInput.x * lookSense.x, lookInput.y * lookSense.y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetMotionInput(InputAction.CallbackContext ctx)
    {
        motionInput = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetReloadInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            reloadPressed = true;
        }
        else if (ctx.performed)
        {
            reloadPressed = false;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            jumpPressed = true;
        }
        else if (ctx.performed)
        {
            jumpPressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetShootInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            shootPressed = true;
        }
        else if (ctx.performed)
        {
            shootPressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetSprintInput(InputAction.CallbackContext ctx)
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
    public void GetDropGrenadeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            dropGrenadePressed = true;
        }
        else if (ctx.performed)
        {
            dropGrenadePressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetThrowGrenadeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            throwGrenadePressed = true;
        }
        else if (ctx.performed)
        {
            throwGrenadePressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetOptionsInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            optionsPressed = true;
        }
        else if (ctx.performed)
        {
            optionsPressed = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetCrouchInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
             crouchPressed = true;
        }
        else if (ctx.performed)
        {
            crouchPressed = false;
        }
    }
}
