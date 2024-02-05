using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachedDeletion : MonoBehaviour
{
    int plr = -1;

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
    }

    internal void init(int id)
    {
        plr = id;
        EventManager.instance.addListener(Events.onPlayerDeath, deathCheck);
        EventManager.instance.addListener(Events.onRoundEnd, roundEnd);
        //int bitmask = (1 << (11 + plr));
        //gameObject.layer = bitmask;


    }
}
