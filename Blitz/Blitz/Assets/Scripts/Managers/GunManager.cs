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
        changeGuns();
    }

    internal void changeGuns()
    {
        gunUsed = Random.Range(0, guns.Length - 1);
        for (int i=0; i< SplitScreenManager.instance.GetPlayers().Count; i++)
        {
            assignGun(i);
        }
    }

    internal void assignGun(int Player)
    {
        Transform plr = SplitScreenManager.instance.GetPlayers()[Player].transform;
        if (plr.GetComponent<PlayerBodyFSM>().playerGun.gameObject != null) Destroy(plr.GetComponent<PlayerBodyFSM>().playerGun.gameObject);
        GameObject gun = Instantiate(guns[gunUsed], new Vector3(0.3f, 1, 0), plr.rotation, plr.GetChild(1));
        gun.GetComponent<Gun>().gunVars.bulletParent = transform;
        plr.GetComponent<PlayerBodyFSM>().assignGun(gun);
    }
}
