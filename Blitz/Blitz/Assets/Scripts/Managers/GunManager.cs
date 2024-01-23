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

        changeGuns();
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

    internal int pickGun()
    {
        return Random.Range(0, guns.Length-1); 
    }

    internal void changeGuns(EventParams param = new EventParams())
    {
        if (!ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RANDOM_GUNS])
        {
            gunUsed = pickGun();
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                assignGun(i);
            }
        } else
        {
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
        Transform plr = SplitScreenManager.instance.GetPlayers()[Player].transform;
        PlayerBodyFSM FSM = plr.GetComponent<PlayerBodyFSM>();

        if (FSM.playerGun.gameObject != null) Destroy(FSM.playerGun.gameObject);

        GameObject gun = Instantiate(guns[gunNumber], plr.GetChild(1));

        gun.transform.localPosition = new Vector3(0f, 1f, 0f);
        gun.transform.forward = FSM.playerBody.forward;

        if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RICOCHET])
        {
            gun.GetComponent<Gun>().RicochetEvent();
        }

        gun.GetComponent<Gun>().gunVars.bulletParent = transform;
        plr.GetComponent<PlayerBodyFSM>().assignGun(gun);
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
