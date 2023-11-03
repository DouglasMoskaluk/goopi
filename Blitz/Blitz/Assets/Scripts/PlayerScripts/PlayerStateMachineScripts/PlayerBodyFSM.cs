using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// 
/// </summary>
public class PlayerBodyFSM : MonoBehaviour
{
    #region Public Variables
    public bool DisplayDebugMessages = false;//whether or not to display debug information
    public PlayerMotionStates currentMotionStateFlag { get; private set; }// the players motion state indicator
    public PlayerActionStates currentActionStateFlag { get; private set; }// the players action state indicator

    public Gun playerGun;

    public TextMeshProUGUI DEBUG_HealthDisplay;
    #endregion

    #region Private Variables
    private CharacterController charController;//ref to character controller
    private Animator anim;// ref to animator
    private PlayerInputHandler input;// ref to input handler
    [SerializeField] private Transform cam;// ref to the camera rotation transform
    [SerializeField] private Transform playerBody;
    private PlayerGrenadeThrower grenadeThrower;// ref to the players grenade thrower component
    [SerializeField] private Transform throwFrom;

    private int health = 100;// the players health
    private const int MAX_HEALTH = 100;//the max health a player can have
    //probably some type of gun reference

    private PlayerMotionState currentMotionState;// the players current motion state
    private PlayerActionState currentActionState;// the players current action state

    private Vector3 knockBackVector = Vector3.zero;

    private float[] damagedByPlayer;
    //private 
    #endregion


    /// <summary>
    /// standard unity awake
    /// called when object is instantiated
    /// </summary>
    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        charController = GetComponent<CharacterController>();
        grenadeThrower = GetComponent<PlayerGrenadeThrower>();

        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);

        damagedByPlayer = new float[4];
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
        //Debug.Log(currentMotionStateFlag);
        currentMotionState.stateUpdate();
        currentActionState.stateUpdate();

        //test respawning
        if (Input.GetKeyDown(KeyCode.K))
        {
            logMessage("Killing player");
            death();
        }

        currentMotionState.transitionCheck();
        currentActionState.transitionCheck();

        DEBUG_HealthDisplay.text = "" + health;

        if (transform.position.y < -10)
        {
            death();
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
            case PlayerMotionStates.KnockBack:
                currentMotionState = new PlayerKnockBackMotionState();
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
            case PlayerActionStates.DropGrenade:
                currentActionState = new PlayerDropGrenadeActionState();
                break;
            case PlayerActionStates.ThrowGrenade:
                currentActionState = new PlayerThrowGrenadeActionState();
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

        return new stateParams(this, anim, charController, input, cam, transform, grenadeThrower, throwFrom, playerGun, playerBody);
    }

    /// <summary>
    /// Way to have plaeyr states log messages into the console
    /// </summary>
    /// <param name="message"> the message to be logged </param>
    public void logMessage(string message)
    {
        Debug.Log("FSM Message: " + message);
    }

    /// <summary>
    /// Alters the player's health by the given value
    /// </summary>
    /// <param name="value"> the value health is altered by</param>
    public void alterHealth(int value)
    {
        health = Mathf.Min(health += value, MAX_HEALTH);
        if (health == MAX_HEALTH)
        {
            for (int i = 0; i < damagedByPlayer.Length; i++)
            {
                damagedByPlayer[i] = 0;
            }
        }
        //health = Mathf.Max(health, 0);
        if (health < 0) death();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="Attacker"></param>
    public void damagePlayer(int value, GameObject Attacker)
    {

        Debug.Log("Player says: Damage Player " + name + " by " + Attacker.name+ " for " + value + " damage");
        if (SplitScreenManager.instance.getPlayerID(Attacker) != -1)
            damagedByPlayer[SplitScreenManager.instance.getPlayerID(Attacker)] += value;
        else Debug.LogError("Player damaged by non-existing player!");
        if ((health -= value) <= 0) death();
    }


    /// <summary>
    /// Player dies
    /// </summary>
    private void death()
    {
        charController.enabled = false;
        Debug.Log("Player Died!");
        transform.position = RespawnManager.instance.getRespawnLocation().position;
        //Heal attackers
        resetHealth();
        charController.enabled = true;
        grenadeThrower.setGrenades(4);
        playerGun.reload();
    }

    /// <summary>
    /// resets the players health back to the max
    /// </summary>
    public void resetHealth()
    {
        alterHealth(MAX_HEALTH * 3);
    }

    /// <summary>
    /// returns the knock back vector of this FSM body
    /// </summary>
    /// <returns></returns>
    public Vector3 getKnockBackVector()
    {
        return knockBackVector;
    }

    /// <summary>
    /// adds a vector to the knock back vector of this FSM body
    /// </summary>
    /// <param name="toAdd"></param>
    public void addKnockBack(Vector3 toAdd)
    {
        knockBackVector += toAdd;
    }

    /// <summary>
    /// sets the knock back vector of this body to newKnockBack
    /// </summary>
    /// <param name="newKnockBack"></param>
    public void setKnockBack(Vector3 newKnockBack)
    {
        knockBackVector = newKnockBack;
    }
}

/// <summary>
/// enum holding all the possible motion states a player can be in
/// </summary>
public enum PlayerMotionStates
{
    None, Walk, Run, Jump, Fall, Slide, Mantle, KnockBack
}

/// <summary>
/// enum holding all the possible action states a player can be in
/// </summary>
public enum PlayerActionStates
{
    None, Idle, Reload, Mantle, Shoot, Run, DropGrenade, ThrowGrenade//shoot might be taken out
}

/// <summary>
/// helper struct responsible for hold references of important components 
/// such as animator, rigidbody etc for states to receive from their FSM
/// </summary>
public struct stateParams
{

    public stateParams(PlayerBodyFSM fsm, Animator an, CharacterController contr, PlayerInputHandler inputH, 
        Transform camera, Transform playerTrans, PlayerGrenadeThrower thrower, Transform throwFrom, Gun pGun, Transform pBody)
    {
        FSM = fsm;
        anim = an;
        controller = contr;
        inputHandler = inputH;
        cam = camera;
        playerTransform = playerTrans;
        gThrower = thrower;
        throwGrenFrom = throwFrom;
        gun = pGun;
        playerBody = pBody;
    }

    public PlayerBodyFSM FSM;
    public Animator anim;
    public CharacterController controller;
    public PlayerInputHandler inputHandler;
    public Transform cam;
    public Transform playerTransform;
    public PlayerGrenadeThrower gThrower;
    public Transform throwGrenFrom;
    public Gun gun;
    public Transform playerBody;
}
