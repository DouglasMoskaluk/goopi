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
        gunUsed = Random.Range(0, guns.Length - 1);
    }

    internal void assignGun(int Player)
    {
        Transform plr = SplitScreenManager.instance.GetPlayers()[Player].transform;
        GameObject gun = Instantiate(guns[gunUsed], new Vector3(0.3f, 1, 0), plr.rotation, plr.GetChild(1));
        plr.GetComponent<PlayerBodyFSM>().assignGun(gun);
    }
}
