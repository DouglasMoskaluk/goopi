using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// 
/// </summary>
public class PlayerBodyFSM : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables
    private CharacterController charController;

    private PlayerMotionState currentMotionState;
    private PlayerActionState currentActionState;

    public PlayerMotionStates currentMotionStateFlag { get; private set; }
    public PlayerActionStates currentActionStateFlag { get; private set; }
   
    #endregion


    /// <summary>
    /// standard unity awake
    /// called when object is instantiated
    /// </summary>
    private void Awake()
    {
  
    }

    /// <summary>
    /// standard unity start
    /// called the before first frame the object is alive 
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// standard unity update
    /// called within unity's update loop
    /// </summary>
    private void Update()
    {
        
    }

    /// <summary>
    /// standard unity fixed update
    /// called within unity's fixed update loop
    /// </summary>
    private void FixedUpdate()
    {
        
    }

    /// <summary>
    /// standard unity late update
    /// called within unity's late update loop
    /// </summary>
    private void LateUpdate()
    {
        
    }

    /// <summary>
    /// called when transitioning from the current motion state to another motion state
    /// </summary>
    public void transitionState(PlayerMotionStates to)
    {
        currentMotionState.onStateExit();

        switch (to)
        {
            case PlayerMotionStates.Walk:

                break;
            case PlayerMotionStates.Crouch:

                break;
            case PlayerMotionStates.Run:

                break;
            case PlayerMotionStates.Slide:

                break;
            case PlayerMotionStates.Fall:

                break;
            case PlayerMotionStates.Jump:

                break;
            case PlayerMotionStates.Mantle:

                break;
        }

        currentMotionState.initState(getFSMInfo());
        currentMotionState.onStateEnter();
    }

    /// <summary>
    /// called when transitioning from the current action state to another action state
    /// </summary>
    public void transitionState(PlayerActionStates to)
    {
        currentActionState.onStateExit();

        switch (to)
        {
            case PlayerActionStates.Idle:

                break;
            case PlayerActionStates.Reload:

                break;
            case PlayerActionStates.Shoot:

                break;
            case PlayerActionStates.Run:

                break;
            case PlayerActionStates.Mantle:

                break;
        }

        currentActionState.initState(getFSMInfo());
        currentActionState.onStateEnter();
    }

    /// <summary>
    /// called when transitioning into a new state 
    /// responsible for collecting all important references from a playerBodyFSM and storing them into a struct
    /// </summary>
    /// <returns> returns struct populated with references from this FSM</returns>
    private stateParams getFSMInfo()
    {

        throw new NotImplementedException();
    }
}

/// <summary>
/// enum holding all the possible motion states a player can be in
/// </summary>
public enum PlayerMotionStates
{
    Walk, Crouch, Run, Jump, Fall, Slide, Mantle
}

/// <summary>
/// enum holding all the possible aciton states a player can be in
/// </summary>
public enum PlayerActionStates
{
    Idle, Reload, Mantle, Shoot, Run//shoot might be taken out
}
