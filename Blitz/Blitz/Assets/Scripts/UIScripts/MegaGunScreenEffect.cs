using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaGunScreenEffect : MonoBehaviour
{
    int plr = -1;

    [SerializeField] GameObject[] screens;

    /*
    void deathCheck(EventParams param = new EventParams())
    {
        if (param.killed == plr)
        {
            Destroy(gameObject);
        }
    }

    void roundEnd(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }*/

    public void Grabbed()
    {
        int player = GetComponentInParent<PlayerBodyFSM>().playerID;

        screens[player].SetActive(true);

        //EventManager.instance.addListener(Events.onPlayerDeath, deathCheck);
        //EventManager.instance.addListener(Events.onEventEnd, roundEnd);
    }
}
