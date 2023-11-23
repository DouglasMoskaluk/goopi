using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager instance;

    [SerializeField]
    private GameObject[] guns;
    int gunUsed;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        
    }

    private void Start()
    {
        RoundManager.instance.onRoundReset.AddListener(destroyParentedWorldObjects);
        RoundManager.instance.onRoundReset.AddListener(changeGuns);
        changeGuns();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            nextGun();
        }
    }

    public void destroyParentedWorldObjects()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    internal void changeGuns()
    {
        gunUsed = Random.Range(0, guns.Length - 1);
        for (int i=0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
        {
            assignGun(i);
        }
    }

    internal void assignGun(int Player)
    {
        Transform plr = SplitScreenManager.instance.GetPlayers()[Player].transform;
        PlayerBodyFSM FSM = plr.GetComponent<PlayerBodyFSM>();
        if (FSM.playerGun.gameObject != null) Destroy(FSM.playerGun.gameObject);
        GameObject gun = Instantiate(guns[gunUsed], plr.transform.position + new Vector3(0.35f, 1, 0), FSM.playerBody.rotation, plr.GetChild(1));
        gun.GetComponent<Gun>().gunVars.bulletParent = transform;
        plr.GetComponent<PlayerBodyFSM>().assignGun(gun);
    }

    internal void nextGun()
    {
        gunUsed = (gunUsed++) % guns.Length;
        for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
        {
            assignGun(gunUsed);
        }
    }
}
