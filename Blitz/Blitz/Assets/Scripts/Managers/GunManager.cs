using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GunManager : MonoBehaviour
{
    public static GunManager instance;

    [SerializeField]
    private GameObject[] guns;
    int gunUsed;
    internal int GunUsed { get { return gunUsed; } }
    private int[] gunOrder;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        //RoundManager.instance.onRoundReset.AddListener(destroyParentedWorldObjects);
        //RoundManager.instance.onRoundReset.AddListener(changeGuns);

        gunOrder = new int[GameManager.instance.maxRoundsPlayed];

        EventManager.instance.addListener(Events.onRoundStart, destroyParentedWorldObjects);
        EventManager.instance.addListener(Events.onRoundStart, changeGuns);
        EventManager.instance.addListener(Events.onEventStart, initModifier);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            nextGun();
        }
    }

    public void destroyParentedWorldObjects(EventParams param = new EventParams())
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    internal void SetLockerroomGuns()
    {
        gunUsed = (int)Gun.GunType.NERF - 1;

        for (int i = 0; i < gunOrder.Length; i++)
        {
            gunOrder[i] = Random.Range(0, (int)Gun.GunType.BOOMSTICK-2);
            for (int j = 0; j < i; j++)
            {
                if (gunOrder[j] == gunOrder[i])
                {
                    i--;
                    continue;
                }
            }
        }
    }

    internal int pickGun()
    {
        if (gunUsed != 5) return gunOrder[RoundManager.instance.getRoundNum()]; 
        return gunOrder[(int)Random.Range(0, RoundManager.instance.getRoundNum()-1)]; 
    }

    internal void changeGuns(EventParams param = new EventParams())
    {
        if (!ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RANDOM_GUNS])
        {
            int roundNum = RoundManager.instance.getRoundNum();
            gunUsed = gunOrder[roundNum];
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                assignGun(i);
            }
        } else
        {
            gunUsed = 5;
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                int gunToUse = gunOrder[(int)Random.Range(0, RoundManager.instance.getRoundNum()-1)];
                assignGun(i, gunToUse);
            }
        }
    }

    bool repeatGun(int chosenGun)
    {
        if (chosenGun == 5) return false;
        for (int i = 0; i < RoundManager.instance.getRoundNum(); i++)
        {
            if ((int)gunOrder[i] == chosenGun) return true;
        }
        return false;
    }

    internal void assignGun(int Player)
    {
        assignGun(Player, gunUsed);
    }

    internal void assignGun(int Player, int gunNumber)
    {
        Transform plr = SplitScreenManager.instance.GetPlayers(Player).transform;
        PlayerBodyFSM FSM = plr.GetComponent<PlayerBodyFSM>();

        if (FSM.playerGun.gameObject != null) Destroy(FSM.playerGun.gameObject);

        GameObject gun = Instantiate(guns[gunNumber], plr.Find("Otter/OtterCharacter/Bone.26/Bone.10/Bone.09"));
        ParentConstraint constraint = gun.GetComponent<ParentConstraint>();
        ConstraintSource source = new ConstraintSource();
        //source.weight = 0.34f;
        //source.sourceTransform = plr.Find("Otter/OtterCharacter/Bone.26/Bone.10/Bone.09/Bone.11");
        //constraint.AddSource(source);
        //constraint.constraintActive = true;
        //gun.transform.SetParent(plr.Find("Otter/OtterCharacter/Bone.26/Bone.10/Bone.09"), true);

        Gun gunScript = gun.GetComponent<Gun>();
        gun.transform.localPosition = gunScript.getInitPosition();
        gun.transform.localEulerAngles = gunScript.getInitRotation();

        gunScript.gunVars.bulletParent = transform;

        //gun.transform.localPosition = new Vector3(0f, 0.001044071f, 0f);
        //gun.transform.forward = FSM.playerBody.forward;

        if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RICOCHET])
        {
            gun.GetComponent<Gun>().bulletVars.shouldBounce = true;
        }
        
        FSM.assignGun(gun);
    }


    internal void initModifier(EventParams param = new EventParams())
    {
        for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++) 
            SplitScreenManager.instance.GetPlayers(i).GetComponent<PlayerBodyFSM>().playerGun.RicochetEvent(ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RICOCHET]);
    }

    internal void nextGun()
    {
        gunUsed = (gunUsed+1) % guns.Length;
        for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
        {
            assignGun(i);
        }
    }
}
