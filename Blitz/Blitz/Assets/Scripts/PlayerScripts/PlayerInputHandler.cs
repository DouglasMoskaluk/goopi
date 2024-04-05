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

    public Vector2 sensitivityInput { get; private set; }

    public Vector2 UIMoveInput { get; private set; }


    [SerializeField] public Vector2 lookSense;
    #endregion

    #region bools

    private PlayerInputActions input;

    public bool jumpPressed { get; private set; } = false;
    public bool shootPressed { get; private set; } = false;
    public bool toggleSprint { get; private set; } = false;

    public bool toggleSlide { get; private set; } = false;

    public bool reloadPressed { get; private set; } = false;
    public bool dropGrenadePressed { get; private set;} = false;
    public bool throwGrenadePressed { get; private set; } = false;
    public bool optionsPressed { get; private set; } = false;

    public bool UIMashPressed { get; private set; } = false;
    #endregion


    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void FixedUpdate()
    {
        UIMashPressed = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getLookInput(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
        Vector2 newLookInput = new Vector2(Mathf.Pow((lookInput.x * lookSense.x), 2), Mathf.Pow((lookInput.y * lookSense.y), 2));


        if (lookInput.x < 0)
        {
            newLookInput.x *= -1;
        }
        if (lookInput.y < 0)
        {
            newLookInput.y *= -1;
        }

        lookInput = newLookInput;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void getMotionInput(InputAction.CallbackContext ctx)
    {
        motionInput = ctx.ReadValue<Vector2>();
        Vector2 newMotion = new Vector2(Mathf.Pow(motionInput.x, 2f), Mathf.Pow(motionInput.y, 2f));

        if (motionInput.x < 0)
        {
            newMotion.x *= -1;
        }
        if (motionInput.y < 0)
        {
            newMotion.y *= -1;
        }

        motionInput = newMotion;

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

    public void getUIMashInput(InputAction.CallbackContext ctx)
    {
        
        if (ctx.started)
        {
            UIMashPressed = true;
            //Debug.Log("MASH PRESSED");
        }
        else
        {
            UIMashPressed = false;
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
    public void slideToggle(InputAction.CallbackContext ctx)
    {
        if (toggleSlide)
        {
            if (ctx.started)
            {
                toggleSlide = !toggleSlide;
            }
        }
        else
        {
            if (ctx.started && motionInput.y >= 0)
            {
                toggleSlide = !toggleSlide;
            }
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

    public void getSensitivityInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            sensitivityInput = ctx.ReadValue<Vector2>();
        }
        else
        {
            sensitivityInput = new Vector2(0,0);
        }
    }

    public void getUIInput(InputAction.CallbackContext ctx)
    {
        UIMoveInput = ctx.ReadValue<Vector2>();
    }

    public void resetSlide()
    {
        toggleSlide = false;
    }
}
