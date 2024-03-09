using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager instance;

    [SerializeField]
    private GameObject[] guns;
    int gunUsed;
    internal int GunUsed { get { return gunUsed; } }


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        
    }

    private void Start()
    {
        //RoundManager.instance.onRoundReset.AddListener(destroyParentedWorldObjects);
        //RoundManager.instance.onRoundReset.AddListener(changeGuns);

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
    }

    internal int pickGun()
    {
        return Random.Range(0, guns.Length-1); 
    }

    internal void changeGuns(EventParams param = new EventParams())
    {
        if (!ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RANDOM_GUNS])
        {
            int thisRoundGun;
            do
            {
                thisRoundGun = pickGun();
            } while (thisRoundGun == gunUsed);
            gunUsed = thisRoundGun;
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                assignGun(i);
            }
        } else
        {
            gunUsed = 5;
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                assignGun(i, pickGun());
            }
        }
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

        //FSM.SetPlayerSpineValue(0.5f);

        GameObject gun = Instantiate(guns[gunNumber], plr.Find("Otter/OtterCharacter/Bone.26/Bone.10/Bone.09/Bone.11"));
        //gun.transform.SetParent(plr.Find("Otter/OtterCharacter/Bone.26/Bone.10/Bone.09/Bone.11"), true);

        gun.transform.localPosition = new Vector3(0f, 0f, 0f);
        gun.transform.forward = FSM.playerBody.forward;

        gun.GetComponent<Gun>().gunVars.bulletParent = transform;

        if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RICOCHET])
        {
            gun.GetComponent<Gun>().bulletVars.bounces = true;
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
