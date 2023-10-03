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
    public bool DisplayDebugMessages = false;
    #endregion

    #region Private Variables
    private CharacterController charController;
    private Animator anim;
    //probably some type of gun reference

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
        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);
    }

    /// <summary>
    /// standard unity update
    /// called within unity's update loop
    /// </summary>
    private void Update()
    {
        currentMotionState.stateUpdate();
        currentActionState.stateUpdate();
    }

    /// <summary>
    /// standard unity fixed update
    /// called within unity's fixed update loop
    /// </summary>
    private void FixedUpdate()
    {
        currentMotionState.stateFixedUpdate();
        currentActionState.stateFixedUpdate();
    }

    /// <summary>
    /// standard unity late update
    /// called within unity's late update loop
    /// </summary>
    private void LateUpdate()
    {
        currentMotionState.stateLateUpdate();
        currentActionState.stateLateUpdate();

        //placement of this is still in the air
        //currently placed in late update so it happens after all other updates are executed but we will see
        currentMotionState.transitionCheck();
        currentActionState.transitionCheck();
    }

    /// <summary>
    /// called when transitioning from the current MOTION state to another motion state
    /// </summary>
    public void transitionState(PlayerMotionStates to)
    {
        //guard against transitioning into the same state
        if (to == currentMotionStateFlag) { return; }

        //exit current state if exists
        if (currentMotionState != null) { currentMotionState.onStateExit(); }
        
        //switch to new state
        switch (to)
        {
            case PlayerMotionStates.Walk:
                currentMotionState = new PlayerWalkMotionState();
                currentMotionStateFlag = PlayerMotionStates.Walk;
                break;
            case PlayerMotionStates.Crouch:
                currentMotionState = new PlayerCrouchMotionState();
                currentMotionStateFlag = PlayerMotionStates.Crouch;
                break;
            case PlayerMotionStates.Run:
                currentMotionState = new PlayerRunMotionState();
                currentMotionStateFlag = PlayerMotionStates.Run;
                break;
            case PlayerMotionStates.Slide:
                currentMotionState = new PlayerSlideMotionState();
                currentMotionStateFlag = PlayerMotionStates.Slide;
                break;
            case PlayerMotionStates.Fall:
                currentMotionState = new PlayerFallMotionState();
                currentMotionStateFlag = PlayerMotionStates.Fall;
                break;
            case PlayerMotionStates.Jump:
                currentMotionState = new PlayerJumpMotionState();
                currentMotionStateFlag = PlayerMotionStates.Jump;
                break;
            case PlayerMotionStates.Mantle:
                currentMotionState = new PlayerMantleMotionState();
                currentMotionStateFlag = PlayerMotionStates.Mantle;
                break;
        }

        //initialize new state
        currentMotionState.initState(getFSMInfo());
        currentMotionState.onStateEnter();
        
        //debug output
        if (DisplayDebugMessages) { Debug.Log("Transitioned into " + currentMotionStateFlag); }
    }

    /// <summary>
    /// called when transitioning from the current ACTION state to another action state
    /// </summary>
    public void transitionState(PlayerActionStates to)
    {
        //guard against transitioning into the same state
        if (to == currentActionStateFlag) { return; }

        //exit current state if exists
        if (currentMotionState != null) { currentActionState.onStateExit(); }

        //switch to new state
        switch (to)
        {
            case PlayerActionStates.Idle:
                currentActionState = new PlayerIdleActionState();
                currentActionStateFlag = PlayerActionStates.Idle;
                break;
            case PlayerActionStates.Reload:
                currentActionState = new PlayerReloadActionState();
                currentActionStateFlag = PlayerActionStates.Reload;
                break;
            case PlayerActionStates.Shoot:
                currentActionState = new PlayerShootActionState();
                currentActionStateFlag = PlayerActionStates.Shoot;
                break;
            case PlayerActionStates.Run:
                currentActionState = new PlayerRunActionState();
                currentActionStateFlag = PlayerActionStates.Run;
                break;
            case PlayerActionStates.Mantle:
                currentActionState = new PlayerMantleActionState();
                currentActionStateFlag = PlayerActionStates.Mantle;
                break;
        }

        //initialize new state
        currentActionState.initState(getFSMInfo());
        currentActionState.onStateEnter();

        //debug output
        if (DisplayDebugMessages) { Debug.Log("Transitioned into " + currentActionStateFlag); }
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

/// <summary>
/// helper struct responsible for hold references of important components 
/// such as animator, rigidbody etc for states to receive from their FSM
/// </summary>
public struct stateParams
{
    public PlayerBodyFSM FSM;
    public Animator anim;
}
