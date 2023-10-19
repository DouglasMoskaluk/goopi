using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private PlayerInputHandler input;
    [SerializeField] private Transform camHolder;
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
        input = GetComponent<PlayerInputHandler>();
        charController = GetComponent<CharacterController>();

        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);
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
        Debug.Log(currentMotionStateFlag);
        currentMotionState.stateUpdate();
        currentActionState.stateUpdate();

        if (Input.GetKeyDown(KeyCode.K))
        {
            LogMessage("Killing player");
            charController.enabled = false;
            transform.position = RespawnManager.instance.GetRespawnLocation().position;
            charController.enabled = true;
        }
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
    /// MOTION |
    /// called when transitioning from the current MOTION state to another motion state
    /// </summary>
    public void transitionState(PlayerMotionStates to)
    {
        //guard against transitioning into the same state
        if (to == currentMotionStateFlag) {
            Debug.Log("trying to transition into current state: aborting transition request");
            return; }

        //exit current state if exists
        if (currentMotionState != null) { currentMotionState.onStateExit(); }
        
        //switch to new state
        switch (to)
        {
            case PlayerMotionStates.Walk:
                currentMotionState = new PlayerWalkMotionState();
                break;
            case PlayerMotionStates.Crouch:
                currentMotionState = new PlayerCrouchMotionState();
                break;
            case PlayerMotionStates.Run:
                currentMotionState = new PlayerRunMotionState();
                break;
            case PlayerMotionStates.Slide:
                currentMotionState = new PlayerSlideMotionState();
                break;
            case PlayerMotionStates.Fall:
                currentMotionState = new PlayerFallMotionState();
                break;
            case PlayerMotionStates.Jump:
                currentMotionState = new PlayerJumpMotionState();
                break;
            case PlayerMotionStates.Mantle:
                currentMotionState = new PlayerMantleMotionState();
                break;
        }

        //initialize new state
        currentMotionStateFlag = to;
        currentMotionState.initState(getFSMInfo());
        currentMotionState.onStateEnter();
        
        //debug output
        if (DisplayDebugMessages) { Debug.Log("Transitioned into " + currentMotionStateFlag); }
    }

    /// <summary>
    /// ACTION |
    /// called when transitioning from the current ACTION state to another action state
    /// </summary>
    public void transitionState(PlayerActionStates to)
    {
        //guard against transitioning into the same state
        if (to == currentActionStateFlag) { return; }

        //exit current state if exists
        if (currentActionState != null) { currentActionState.onStateExit(); }

        //switch to new state
        switch (to)
        {
            case PlayerActionStates.Idle:
                currentActionState = new PlayerIdleActionState();
                break;
            case PlayerActionStates.Reload:
                currentActionState = new PlayerReloadActionState();
                break;
            case PlayerActionStates.Shoot:
                currentActionState = new PlayerShootActionState();
                break;
            case PlayerActionStates.Run:
                currentActionState = new PlayerRunActionState();
                break;
            case PlayerActionStates.Mantle:
                currentActionState = new PlayerMantleActionState();
                break;
        }

        //initialize new state
        currentActionStateFlag = to;
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

        return new stateParams(this, anim, charController, input, camHolder, transform);
    }

    public void LogMessage(string message)
    {
        Debug.Log("FSM Message: " + message);
    }
}

/// <summary>
/// enum holding all the possible motion states a player can be in
/// </summary>
public enum PlayerMotionStates
{
    None, Walk, Crouch, Run, Jump, Fall, Slide, Mantle
}

/// <summary>
/// enum holding all the possible aciton states a player can be in
/// </summary>
public enum PlayerActionStates
{
    None, Idle, Reload, Mantle, Shoot, Run//shoot might be taken out
}

/// <summary>
/// helper struct responsible for hold references of important components 
/// such as animator, rigidbody etc for states to receive from their FSM
/// </summary>
public struct stateParams
{

    public stateParams(PlayerBodyFSM fsm, Animator an, CharacterController contr, PlayerInputHandler inputH, Transform camHold, Transform playerTrans)
    {
        FSM = fsm;
        anim = an;
        controller = contr;
        inputHandler = inputH;
        camholder = camHold;
        playerTransform = playerTrans;
    }

    public PlayerBodyFSM FSM;
    public Animator anim;
    public CharacterController controller;
    public PlayerInputHandler inputHandler;
    public Transform camholder;
    public Transform playerTransform;
}
