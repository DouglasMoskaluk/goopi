using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using Cinemachine;
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

    public int skinID;

    #endregion

    #region Private Variables
    private CharacterController charController;//ref to character controller
    [SerializeField] private Animator anim;// ref to animator
    private PlayerInputHandler input;// ref to input handler
    //private RagDollHandler ragdoll;// ref to ragdoll handler
    [SerializeField] private Transform cam;// ref to the camera rotation transform
    [SerializeField] internal Transform playerBody;
    private PlayerGrenadeThrower grenadeThrower;// ref to the players grenade thrower component
    [SerializeField] private Transform throwFrom;
    private FSMVariableHolder variableHolder;
    [SerializeField] private PlayerRigHolder rigHolder;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private GameObject healthPack;
    [SerializeField] private GameObject ragdollBody;
    [SerializeField] private GameObject gunRagdollBody;
    [SerializeField] private GameObject crownRagdollBody;
    [SerializeField] private PlayerUIHandler uiHandler;
    [SerializeField] private GameObject playerCrown;
    [SerializeField] private PlayerGrenadeArcRenderer grenadeArcRenderer;
    [SerializeField] private PlayerHeadMotion headMotion;
    [SerializeField] private PlayerMotionDustParticles dustParticles;
    [SerializeField] private CinemachineFreeLook cine;
    private Transform gunPositionRef;
   

    private CinemachineFreeLook freelookCam; //freelook brain reference
    private Transform camRotatePoint;

    private int health = 100;// the players health
    private const int MAX_HEALTH = 100;//the max health a player can have

    private PlayerMotionState currentMotionState;// the players current motion state
    private PlayerActionState currentActionState;// the players current action state

    private Vector3 knockBackVector = Vector3.zero;

    private bool deathCheck = false;

    private int mostRecentAttacker = -1;

    GroundRayCast rayInfo;
    private float rayCastRadius = 0.5f;
    public float groundRayCastOffset = -0.8f;

    private int winDanceNum = -1;

    private IEnumerator deathCoroutine;

    //private 
    #endregion

    /// <summary>
    /// standard unity awake
    /// called when object is instantiated
    /// </summary>
    private void Awake()
    {
        //ragdoll = GetComponent<RagDollHandler>();
        input = GetComponent<PlayerInputHandler>();
        charController = GetComponent<CharacterController>();
        grenadeThrower = GetComponent<PlayerGrenadeThrower>();
        variableHolder = GetComponent<FSMVariableHolder>();
        freelookCam = transform.GetChild(2).GetComponent<CinemachineFreeLook>();

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
        EventManager.instance.addListener(Events.onGameEnd, resetFSM);
        EventManager.instance.addListener(Events.onPlayerDeath, resetFSMOnDeath);
        rigHolder.gameObject.GetComponent<Rig>().weight = 1.0f;
        camRotatePoint = transform.GetChild(3);
        gunPositionRef = transform.Find("Otter/OtterCharacter/Bone.26/Bone.10/Bone.09/Bone.11").transform;
        deathCoroutine = deathCoro(null);
    }

    public void SetGrenadeArcRendererLayer(int layer)
    {
        grenadeArcRenderer.gameObject.layer = layer;
    }

    public PlayerUIHandler GetUIHandler()
    {
        return uiHandler;
    }
    public void ForceAnimatorUpdate()
    {
        anim.Update(0);
    }
    public void EnablePlayerCamera()
    {
        cam.GetComponent<Camera>().enabled = enabled;
    }

    public void DisablePlayerCamera(bool disableVCamToo = false)
    {
        cam.GetComponent<Camera>().enabled = false;
        freelookCam.enabled = !disableVCamToo;
    }

    public void RotateBody(Quaternion rotateTo)
    {
        playerBody.rotation = rotateTo;
    }

    public void DisablePlayerUI()
    {
        uiHandler.gameObject.SetActive(false);
    }

    public void enableHeadMotion()
    {
        headMotion.enabled = true;
    }

    public void DisableGun()
    {
        playerGun.gameObject.SetActive(false);
        //rigBuilder.layers.ForEach(layer => layer.rig.weight = 0f);
    }

    public void AllowWinAnimation()
    {
        anim.SetLayerWeight(0, 0);
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        anim.SetLayerWeight(3, 0);
        anim.SetLayerWeight(4, 1);
        anim.SetInteger("DanceType", winDanceNum);
    }
    public void SetWinAnimNumber(int num)
    {
        winDanceNum = num;
    }

    public void SetCameraLookAt(Transform at)
    {
        freelookCam.LookAt = at;
    }

    public void SetBodyRotToCamera()
    {
        currentMotionState.RotateBodyToCamera();
    }

    public void SetPlayerSpineValue(float value)
    {
        freelookCam.m_YAxis.Value = value;
        GetComponent<RotateSpineWithCamera>().LateUpdate();
    }

    public void resetFSM(EventParams param = new EventParams())
    {
        Debug.Log("resetfsm");
        resetHealth();
        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);
        knockBackVector = Vector3.zero;
        dustParticles.hide();
        grenadeArcRenderer.DisableRendering();
        resetReloadAnim();
        if (playerGun != null) playerGun.instantReload();
    }

    public void resetReloadAnim()
    {
        rigHolder.enableRig();
        uiHandler.setReloadIndicatorVisible(false);
        switch (anim.GetInteger("HeldGun")) {
            case 1:
                anim.Play("GoopReloadReset", 1, 0);
                break;
            case 2:
                anim.Play("CrossbowReloadReset", 1, 0);
                break;
            case 3:
                anim.Play("PlungerReloadReset", 1, 0);
                break;
            case 4:
                anim.Play("FishReloadReset", 1, 0);
                break;
             
        }
        
    }

    public void resetFSMOnDeath(EventParams param)
    {
        if (param.killed == playerID)
        {
            resetFSM();
        }
    }

    public void SetAlive()
    {
        StopCoroutine(deathCoroutine);
        resetHealth();
        playerUI.Alive();
        charController.enabled = true;
        grenadeThrower.setGrenades(grenadeThrower.MaxHeldGrenades);
        playerGun.instantReload();
        deathCheck = false;
        //EventManager.instance.invokeEvent(Events.onPlayerRespawn, new EventParams(playerID));

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetCrownVisibility(bool isVisible)
    {
        playerCrown.SetActive(isVisible);
    }

    /// <summary>
    /// standard unity update
    /// called within unity's update loop
    /// </summary>
    private void Update()
    {
        rayInfo.rayHit = Physics.SphereCast(transform.position + charController.center, rayCastRadius, Vector3.down, out rayInfo.rayHitResult , charController.center.y + groundRayCastOffset);

        currentMotionState.stateUpdate();
        currentActionState.stateUpdate();

        //test respawning
        if (Input.GetKeyDown(KeyCode.K))
        {
            logMessage("Killing players");
            death(Vector3.up, Vector3.zero);
        }

        currentMotionState.transitionCheck();
        currentActionState.transitionCheck();

        if (transform.position.y < -10)
        {
            damagePlayer(100, -1, Vector3.zero, Vector3.zero);
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
            //Debug.Log("trying to transition into current state: aborting transition request");
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

    public GroundRayCast GetGroundRayCastInfo()
    {
        return rayInfo;
    }

    /// <summary>
    /// called when transitioning into a new state 
    /// responsible for collecting all important references from a playerBodyFSM and storing them into a struct
    /// </summary>
    /// <returns> returns struct populated with references from this FSM</returns>
    private stateParams getFSMInfo()
    {

        return new stateParams(this, anim, charController, input, cam, transform, grenadeThrower, 
            throwFrom, playerGun, playerBody, variableHolder, rigHolder, grenadeArcRenderer, dustParticles, cine, uiHandler);
    }

    /// <summary>
    /// Way to have player states log messages into the console
    /// </summary>
    /// <param name="message"> the message to be logged </param>
    public void logMessage(string message)
    {
        //Debug.Log("FSM Message: " + message);
    }

    /// <summary>
    /// Alters the player's health by the given value
    /// </summary>
    /// <param name="value"> the value health is altered by</param>
    public void alterHealth(int value)
    {
        health = Mathf.Min(health += value, MAX_HEALTH);
        //health = Mathf.Max(health, 0);
        if (health < 0) death(Vector3.zero, Vector3.zero);
    }


    public void newAttacker(int attackerId)
    {
        if (attackerId != -1 && !deathCheck && attackerId != playerID)
        {
            mostRecentAttacker = attackerId;
        }
    }
     
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="Attacker"></param>
    public void damagePlayer(int value, int attackerId, Vector3 deathVelocity, Vector3 deathSource)
    {
        //Debug.Log("Player says: Damage Player " + name + " by " + Attacker.name+ " for " + value + " damage");
        if (health > 0)
        {
            playerUI.playerGotDamaged();
            newAttacker(attackerId);
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLAYER_HURT);

            if (attackerId != -1 && attackerId != playerID)
            {
                PlayerBodyFSM Attacker = SplitScreenManager.instance.GetPlayers(attackerId);
                Attacker.playerUI.playerGotHit();
                if (health - value <= 0 && Attacker.playerGun.gunVars.type == Gun.GunType.BOOMSTICK)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_OBLITERATED);
                    uiHandler.Obliterated();
                }
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
                death(deathVelocity, deathSource);
            }
            if (health <= 30)
            {
                playerUI.ShowLowHealth();
            }
        }
    }

    /// <summary>
    /// Player dies
    /// </summary>
    private void death(Vector3 deathDirection, Vector3 deathPos)
    {
        StopCoroutine(deathCoroutine);
        //Debug.Log("This player is dying. Previously " + deathCheck);
        //ragdollDeathStart();
        if (!deathCheck)
        {
            SplitScreenManager.instance.SetCrowns();// <- ik this is a weird way to do it and using in on the ondeath event is better ut i think that would require some reworking of the events which we should do but i dont have enough time rn
            deathCheck = true;
            transitionState(PlayerActionStates.Death);
            transitionState(PlayerMotionStates.Death);
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLAYER_DEATH);
            GameObject newRagdollBody = Instantiate(ragdollBody, transform.position, Quaternion.identity);
            gunPositionRef = transform.Find("Otter/OtterCharacter/Bone.26/Bone.10/Bone.09").transform.GetChild(1);
            GameObject newGunRagdoll = Instantiate(gunRagdollBody, gunPositionRef.position, gunPositionRef.transform.rotation);
            RumbleHandler newRumble = transform.GetComponent<RumbleHandler>();
            newRumble.DeathRumble();

            //tranfer bullets on body to ragdoll
            //if(transform.childCount > 5) //there are bullets on player
            //{
            //    for(int i = 5;i < transform.childCount; i++)
            //    {
            //        transform.GetChild(i).transform.parent = newRagdollBody.transform;
            //    }
            //}


            Vector3 playerVelocity = transform.GetComponent<CharacterController>().velocity;

            RagDollHandler newRagDollHandler = newRagdollBody.transform.GetComponent<RagDollHandler>();
            newGunRagdoll.transform.GetComponent<GunRagdoll>().InitializeGunRagdoll((int)playerGun.gunVars.type -1, playerVelocity);
            newGunRagdoll.transform.GetComponent<GunRagdoll>().deathForce(deathDirection);


            freelookCam.m_LookAt = newRagDollHandler.camRotatePoint;
            freelookCam.m_Follow = newRagDollHandler.camRotatePoint;

            if(playerCrown.activeInHierarchy)
            {
                GameObject crownRagdoll = Instantiate(crownRagdollBody, playerCrown.transform.position, playerCrown.transform.rotation);
                crownRagdoll.transform.GetComponent<CrownRagdoll>().InitializeRagdoll(playerVelocity);
                crownRagdoll.transform.GetComponent<CrownRagdoll>().DeathForce(deathDirection);
            }

            Transform[] boneList = transform.GetChild(1).GetComponent<BoneRenderer>().transforms;
            //Vector3 playerVelocity = transform.GetComponent<CharacterController>().velocity;
            newRagDollHandler.InitializeRagdoll(modelID, skinID, boneList, playerVelocity);

            //if (transform.childCount > 5) //there are bullets on player
            //{
            //    List<GameObject> newBullets = new List<GameObject>();
            //    List<int> killerID = new List<int>();

            //    for (int i = 5; i < transform.childCount; i++)
            //    {
            //        if (transform.GetChild(i).gameObject.name.Contains("Bomb") != true && transform.GetChild(i).gameObject.name.Contains("VFX_BasicExplosion_ICICLE") != true)
            //        {
            //            Debug.Log("ISNT A BOMD" + transform.GetChild(i).gameObject.name);
            //            Debug.Log("ISNT A BOMD");
            //            newBullets.Add(transform.GetChild(i).gameObject);
            //            if (transform.GetChild(i).GetComponent<Plunger>())
            //            {
            //                killerID.Add(transform.GetChild(i).GetComponent<Plunger>().Owner);
            //            }
            //            else if (transform.GetChild(i).GetComponent<Explosion>())
            //            {
            //                killerID.Add(transform.GetChild(i).GetComponent<Explosion>().Owner);
            //            }
            //            else
            //            {
            //                killerID.Add(-1);
            //            }
            //            //killerID.Add(transform.GetChild(i).GetComponent<Bullet>().bulletVars.owner);
            //        }
            //        //transform.GetChild(i).transform.parent = newRagdollBody.transform;
            //    }
            //    newRagDollHandler.SetBulletArrayList(newBullets, killerID);
            //}

            newRagDollHandler.DeathForce(deathDirection, deathPos);
            deathCoroutine = deathCoro(newRagDollHandler);
            StartCoroutine(deathCoroutine);
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

    IEnumerator deathCoro(RagDollHandler ragdoll)
    {
        for(int i = 0;i < transform.GetChild(1).childCount; i++)
        {
            transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }

        charController.enabled = false;
        //Debug.Log("Player Died!");
        //Heal attackers
        resetHealth();
        playerUI.StopDamagedCoroutine();
        playerUI.HideLowHealth();
        playerUI.Dead();
        EventManager.instance.invokeEvent(Events.onPlayerDeath, new EventParams(playerID, mostRecentAttacker));
        mostRecentAttacker = -1;
        yield return new WaitForEndOfFrame();

        if (transform.childCount > 5) //there are bullets on player
        {
            List<GameObject> newBullets = new List<GameObject>();
            List<int> killerID = new List<int>();
            for (int i = 5; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).gameObject.name.Contains("Bomb") != true && transform.GetChild(i).gameObject.name.Contains("VFX_BasicExplosion_ICICLE") != true)
                {
                    newBullets.Add(transform.GetChild(i).gameObject);
                    if (transform.GetChild(i).GetComponent<Plunger>())
                    {
                        killerID.Add(transform.GetChild(i).GetComponent<Plunger>().Owner);
                    }
                    else if (transform.GetChild(i).GetComponent<Explosion>())
                    {
                        killerID.Add(transform.GetChild(i).GetComponent<Explosion>().Owner);
                    } else
                    {
                        killerID.Add(-1);
                    }
                    //killerID.Add(transform.GetChild(i).GetComponent<Bullet>().bulletVars.owner);
                }
            }
            ragdoll.SetBulletArrayList(newBullets, killerID);
        }

        Instantiate(healthPack, transform.position, Quaternion.identity);
        //ragdoll.EnableRagdoll();
        yield return new WaitForSeconds(2.0f);

        freelookCam.m_LookAt = camRotatePoint;
        freelookCam.m_Follow = camRotatePoint;

        //ragdoll.DisableRagdoll();
        //transform.position = RespawnManager.instance.getRespawnLocation().position;
        Transform newPos;
        if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.FLOOR_IS_LAVA] && RespawnManager.instance.getLavaSpawnLocation() != null)
        {
            newPos = RespawnManager.instance.getLavaSpawnLocation();
        } else
        {
            newPos = RespawnManager.instance.getRespawnLocation();
        }
        float dist = Vector3.Distance(transform.position, newPos.position);
        transform.position = newPos.position;
        Physics.SyncTransforms();

        resetHealth();
        playerUI.Alive();
        charController.enabled = true;
        grenadeThrower.setGrenades(grenadeThrower.MaxHeldGrenades);
        playerGun.instantReload();
        deathCheck = false;
        EventManager.instance.invokeEvent(Events.onPlayerRespawn, new EventParams(playerID));

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        }

        transitionState(PlayerMotionStates.Walk);
        transitionState(PlayerActionStates.Idle);

        yield return null;
    }


    /// <summary>
    /// refills player health when they hit a health pack
    /// </summary>
    public void refillHealth()
    {
        playerUI.HideLowHealth();
        playerUI.playerGotHealed();
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
        anim.SetInteger("HeldGun", ((int)playerGun.gunVars.type) - 1);
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
        rigHolder.leftArmIKTarget = gunPrefab.transform.Find("GunStancePlacement/WeaponSway/Recoil/LeftTarget");
        rigHolder.leftArmIKHint = gunPrefab.transform.Find("GunStancePlacement/WeaponSway/Recoil/LeftHint");
        rigHolder.leftArmConstraint.data.target = rigHolder.leftArmIKTarget;
        rigHolder.leftArmConstraint.data.hint = rigHolder.leftArmIKHint;

        rigHolder.rightArmIKTarget = gunPrefab.transform.Find("GunStancePlacement/WeaponSway/Recoil/RightTarget");
        rigHolder.rightArmIKHint = gunPrefab.transform.Find("GunStancePlacement/WeaponSway/Recoil/RightHint");
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

    public void SetWalkParticles(bool onOff)
    {
        dustParticles.SetParticlesEnabled(onOff);
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
        Transform pBody, FSMVariableHolder vHolder, PlayerRigHolder rHolder, PlayerGrenadeArcRenderer arc, PlayerMotionDustParticles dustPart,
        CinemachineFreeLook cineFreeLook, PlayerUIHandler playerUIHandler)
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
        arcRender = arc;
        dust = dustPart;
        cine = cineFreeLook;
        uiHandler = playerUIHandler;
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
    public PlayerGrenadeArcRenderer arcRender;
    public PlayerMotionDustParticles dust;
    public CinemachineFreeLook cine;
    public PlayerUIHandler uiHandler;
}

public struct GroundRayCast
{

    public GroundRayCast(bool rayHitBool, RaycastHit rayHitInfoResult)
    {
        rayHit = rayHitBool;
        rayHitResult = rayHitInfoResult;
    }

    public bool rayHit;
    public RaycastHit rayHitResult;
}
