using Cinemachine;
using UnityEngine;
/// <summary>
/// Abstract class representing the highest level of a player state
/// handles all functionality of what happens to a state and how a state interacts within the FSM
/// </summary>
public abstract class PlayerState
{
    protected CharacterController controller;
    protected PlayerBodyFSM FSM;
    protected FSMVariableHolder stateVariableHolder;
    protected Animator anim;
    protected PlayerInputHandler input;
    protected Transform playerTransform;
    protected PlayerGrenadeThrower grenadeThrower;
    protected Transform cam;
    protected Transform throwFrom;
    protected Gun playerGun;
    protected Transform playerBody;
    protected PlayerGrenadeArcRenderer arcRenderer;
    protected PlayerMotionDustParticles dustParticles;
    protected CinemachineFreeLook cine;
    protected PlayerUIHandler uiHandler;

    /// <summary>
    /// is called by FSM when the state is transitioned to
    /// </summary>
    public virtual void onStateEnter()
    {

    }

    /// <summary>
    /// is called by FSM when a state is transitioned from
    /// </summary>
    public virtual void onStateExit()
    {

    }

    /// <summary>
    /// is called by FSM within the update loop when it is the current state of the FSM
    /// </summary>
    public virtual void stateUpdate()
    {

    }

    /// <summary>
    /// is called by FSM within the fixed update loop when it is the current state of the FSM
    /// </summary>
    public virtual void stateFixedUpdate()
    {

    }

    /// <summary>
    /// is called by FSM within the late update loop when it is the current state of the FSM
    /// </summary>
    public virtual void stateLateUpdate()
    {

    }

    /// <summary>
    /// generally called within update loops to check if a transition is to occur, and transitions if it occurs
    /// transition priority is given to transition attempts closer to the top of the function
    /// </summary>
    public virtual void transitionCheck()
    {

    }

    /// <summary>
    /// is called when transitioning into a state
    /// handles passing and configuring needed references from the state and FSM
    /// </summary>
    /// <param name="stateParams"> struct holding parameter references for the state to utilize </param>
    public virtual void initState(stateParams stateParams)
    {
        FSM = stateParams.FSM;
        anim = stateParams.anim;
        controller = stateParams.controller;
        input = stateParams.inputHandler;
        playerTransform = stateParams.playerTransform;
        grenadeThrower = stateParams.gThrower;
        cam = stateParams.cam;
        throwFrom = stateParams.throwGrenFrom;
        playerGun = stateParams.gun;
        playerBody = stateParams.playerBody;
        stateVariableHolder = stateParams.variableHolder;
        arcRenderer = stateParams.arcRender;
        dustParticles = stateParams.dust;
        cine = stateParams.cine;
        uiHandler = stateParams.uiHandler;
    }
}


