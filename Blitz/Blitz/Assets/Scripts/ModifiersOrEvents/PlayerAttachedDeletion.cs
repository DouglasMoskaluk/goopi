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
        //GetComponent<Renderer>().renderingLayerMask = (uint)layers[plr];
        /*switch (plr)
        {
            case 0:
                GetComponent<Renderer>().renderingLayerMask = layers[];
                break;
            case 1:
                GetComponent<Renderer>().renderingLayerMask = (uint)LayerMask.NameToLayer("PlayerDotTwo");
                break;
            case 2:
                GetComponent<Renderer>().renderingLayerMask = (uint)LayerMask.NameToLayer("PlayerDotThree");
                break;
            case 3:
                GetComponent<Renderer>().renderingLayerMask = (uint)LayerMask.NameToLayer("PlayerDotFour");
                break;
        }*/
    }
}
