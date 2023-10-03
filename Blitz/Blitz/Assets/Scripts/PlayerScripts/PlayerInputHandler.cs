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

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        input = new PlayerInputActions();
    }


    private void Update()
    {
        Debug.Log(reloadPressed);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetLookInput(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>().normalized;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetMotionInput(InputAction.CallbackContext ctx)
    {
        motionInput = ctx.ReadValue<Vector2>().normalized;
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

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetShootInput(InputAction.CallbackContext ctx)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetSprintInput(InputAction.CallbackContext ctx)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetDropGrenadeInput(InputAction.CallbackContext ctx)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetThrowGrenadeInput(InputAction.CallbackContext ctx)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetOptionsInput(InputAction.CallbackContext ctx)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void GetCrouchInput(InputAction.CallbackContext ctx)
    {

    }
}
