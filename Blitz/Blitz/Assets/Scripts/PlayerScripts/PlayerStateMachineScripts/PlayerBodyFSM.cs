using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
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

    public PlayerUIHandler playerUI;

    [HideInInspector]
    public int Health { get { return health; } }

    public int playerID;

    public int modelID; //ID for what character model is being used

    #endregion

    #region Private Variables
    private CharacterController charController;//ref to character controller
    [SerializeField] private Animator anim;// ref to animator
    private PlayerInputHandler input;// ref to input handler
    private RagDollHandler ragdoll;// ref to ragdoll handler
    [SerializeField] private Transform cam;// ref to the camera rotation transform
    [SerializeField] internal Transform playerBody;
    private PlayerGrenadeThrower grenadeThrower;// ref to the players grenade thrower component
    [SerializeField] private Transform throwFrom;
    private FSMVariableHolder variableHolder;
    [SerializeField] private PlayerRigHolder rigHolder;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private GameObject healthPack;
    [SerializeField] private GameObject ragdollBody;

    private int health = 100;// the players health
    private const int MAX_HEALTH = 100;//the max health a player can have

    private PlayerMotionState currentMotionState;// the players current motion state
    private PlayerActionState currentActionState;// the players current action state

    private Vector3 knockBackVector = Vector3.zero;

    private bool deathCheck = false;

    private int mostRecentAttacker = -1;
    //private 
    #endregion

    /// <summary>
    /// standard unity awake
    /// called when object is instantiated
    /// </summary>
    private void Awake()
    {
        ragdoll = GetComponent<RagDollHandler>();
        input = GetComponent<PlayerInputHandler>();
        charController = GetComponent<CharacterController>();
        grenadeThrower = GetComponent<PlayerGrenadeThrower>();
        variableHolder = GetComponent<FSMVariableHolder>();

        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);

        playerUI.playerID = playerID;
    }

    /// <summary>
    /// standard unity start
    /// called the before first frame the object is alive 
    /// </summary>
    private void Start()
    {
        //RoundManager.instance.onRoundReset.AddListener(resetFSM);
        EventManager.instance.addListener(Events.onRoundStart, resetFSM);
    }


    public void resetFSM(EventParams param = new EventParams())
    {
        resetHealth();
        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);
        knockBackVector = Vector3.zero;
        if (playerGun != null) playerGun.instantReload();
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
            logMessage("Killing players");
            death();
        }

        currentMotionState.transitionCheck();
        currentActionState.transitionCheck();

        if (transform.position.y < -40)
        {
            damagePlayer(100, -1);
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
            case PlayerMotionStates.Death:
                currentMotionState = new PlayerDeathMotionState();
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
            case PlayerActionStates.Death:
                currentActionState = new PlayerDeathActionState();
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

        return new stateParams(this, anim, charController, input, cam, transform, grenadeThrower, throwFrom, playerGun, playerBody, variableHolder, rigHolder);
    }

    /// <summary>
    /// Way to have player states log messages into the console
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
        //health = Mathf.Max(health, 0);
        if (health < 0) death();
    }


    public void newAttacker(int attackerId)
    {
        if (attackerId != -1 && !deathCheck)
        {
            if (attackerId != playerID) mostRecentAttacker = attackerId;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="Attacker"></param>
    public void damagePlayer(int value, int attackerId)
    {
        //Debug.Log("Player says: Damage Player " + name + " by " + Attacker.name+ " for " + value + " damage");

        playerUI.playerGotdamaged();
        newAttacker(attackerId);
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLAYER_HURT);

        if (attackerId != -1)
        {
            PlayerBodyFSM Attacker = SplitScreenManager.instance.GetPlayers(attackerId);
            Attacker.playerUI.playerGotHit();
        }
        //else Debug.LogError("Player damaged by non-existing player!");
        if ((health -= value) <= 0)
        {
            //player gets kill marker
            //update kill count
            if (mostRecentAttacker != -1 && !deathCheck)
            {
                RoundManager.instance.updateKillCount(mostRecentAttacker);
            }
            death();
        }
        if(health <= 30)
        {
            playerUI.ShowLowHealth();
        }
    }

    /// <summary>
    /// Player dies
    /// </summary>
    private void death()
    {
        //Debug.Log("This player is dying. Previously " + deathCheck);
        //ragdollDeathStart();
        if (!deathCheck)
        {
            deathCheck = true;
            transitionState(PlayerActionStates.Death);
            transitionState(PlayerMotionStates.Death);
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLAYER_DEATH);
            GameObject newRagdollBody = Instantiate(ragdollBody, transform.position, Quaternion.identity);
            Transform[] boneList = transform.GetChild(1).GetComponent<BoneRenderer>().transforms;
            Vector3 playerVelocity = transform.GetComponent<CharacterController>().velocity;
            newRagdollBody.transform.GetComponent<RagDollHandler>().InitializeRagdoll(modelID, boneList, playerVelocity);
            StartCoroutine("deathCoro");
        }

        //charController.enabled = false;
        //Debug.Log("Player Died!");
        //transform.position = RespawnManager.instance.getRespawnLocation().position;
        ////Heal attackers
        //playerUI.StopDamagedCoroutine();
        //playerUI.HideLowHealth();
        //resetHealth();
        //charController.enabled = true;
        //grenadeThrower.setGrenades(4);
        //playerGun.instantReload();

    }

    IEnumerator deathCoro()
    {
        for(int i = 0;i < 5;i++)
        {
            transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }

        charController.enabled = false;
        Debug.Log("Player Died!");
        //Heal attackers
        resetHealth();
        playerUI.StopDamagedCoroutine();
        playerUI.HideLowHealth();
        playerUI.Dead();
        yield return new WaitForEndOfFrame();
        Instantiate(healthPack, transform.position, Quaternion.identity);
        //ragdoll.EnableRagdoll();
        yield return new WaitForSeconds(1.0f);
        //ragdoll.DisableRagdoll();
        //transform.position = RespawnManager.instance.getRespawnLocation().position;
        Transform newPos = RespawnManager.instance.getRespawnLocation();
        float dist = Vector3.Distance(transform.position, newPos.position);
        transform.position = newPos.position;
        EventManager.instance.invokeEvent(Events.onPlayerDeath, new EventParams(playerID, mostRecentAttacker));
        mostRecentAttacker = -1;
        Physics.SyncTransforms();

        resetHealth();
        playerUI.Alive();
        charController.enabled = true;
        grenadeThrower.setGrenades(grenadeThrower.MaxHeldGrenades);
        playerGun.instantReload();
        deathCheck = false;

        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        }

        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);

        yield return null;
    }

    ////STOP USING AFTER INDUSTRY SHOWCASE
    //private void ragdollDeathStart()
    //{
    //    charController.enabled = false;
    //    Debug.Log("Player Died!");
    //    //Heal attackers
    //    playerUI.StopDamagedCoroutine();
    //    playerUI.HideLowHealth();
    //    ragdoll.RagDollDeath();

    //}
    ////STOP USING AFTER INDUSTRY SHOW
    //public void ragdollDeathEnd()
    //{
    //    //transform.position = RespawnManager.instance.getRespawnLocation().position;

    //    resetHealth();
    //    charController.enabled = true;
    //    grenadeThrower.setGrenades(4);
    //    playerGun.instantReload();
    //}

    /// <summary>
    /// refills player health when they hit a health pack
    /// </summary>
    public void refillHealth()
    {
        playerUI.HideLowHealth();
        health = MAX_HEALTH;
    }

    /// <summary>
    /// resets the players health back to the max
    /// </summary>
    public void resetHealth()
    {
        health = MAX_HEALTH;
    }

    /// <summary>
    /// Sets references for the gun
    /// </summary>
    public void assignGun(GameObject myGun)
    {
        playerGun = myGun.GetComponent<Gun>();
        playerUI.gun = playerGun;
        if (currentActionState != null) currentActionState.initState(getFSMInfo()); 
        if (currentMotionState != null) currentMotionState.initState(getFSMInfo());
        setUpGunRig(myGun);
    }

    /// <summary>
    /// retargets ik constraints to proper positions and rotations for new gun gunPrefab
    /// </summary>
    /// <param name="gunPrefab"></param>
    private void setUpGunRig(GameObject gunPrefab)
    {
        rigBuilder.enabled = false;
        rigHolder.leftArmIKTarget = gunPrefab.transform.Find("Recoil/LeftTarget");
        rigHolder.leftArmIKHint = gunPrefab.transform.Find("Recoil/LeftHint");
        rigHolder.leftArmConstraint.data.target = rigHolder.leftArmIKTarget;
        rigHolder.leftArmConstraint.data.hint = rigHolder.leftArmIKHint;

        rigHolder.rightArmIKTarget = gunPrefab.transform.Find("Recoil/RightTarget");
        rigHolder.rightArmIKHint = gunPrefab.transform.Find("Recoil/RightHint");
        rigHolder.rightArmConstraint.data.target = rigHolder.rightArmIKTarget;
        rigHolder.rightArmConstraint.data.hint = rigHolder.rightArmIKHint;
        rigBuilder.enabled = true;
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
    None, Walk, Run, Jump, Fall, Slide, Mantle, KnockBack, Death
}

/// <summary>
/// enum holding all the possible action states a player can be in
/// </summary>
public enum PlayerActionStates
{
    None, Idle, Reload, Mantle, Shoot, Run, DropGrenade, ThrowGrenade, Death
}

/// <summary>
/// helper struct responsible for hold references of important components 
/// such as animator, rigidbody etc for states to receive from their FSM
/// </summary>
public struct stateParams
{

    public stateParams(PlayerBodyFSM fsm, Animator an, CharacterController contr, PlayerInputHandler inputH, 
        Transform camera, Transform playerTrans, PlayerGrenadeThrower thrower, Transform throwFrom, Gun pGun,
        Transform pBody, FSMVariableHolder vHolder, PlayerRigHolder rHolder)
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
        variableHolder = vHolder;
        rigHolder = rHolder;
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
    public FSMVariableHolder variableHolder;
    public PlayerRigHolder rigHolder;
}
