using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttachedDeletion : MonoBehaviour
{
    int plr = -1;

    [SerializeField]
    private List<LayerMask> renderLayers;

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
        int layerToAdd = (int)Mathf.Log(renderLayers[plr].value, 2);
        transform.gameObject.layer = layerToAdd;
        EventManager.instance.addListener(Events.onPlayerDeath, deathCheck);
        EventManager.instance.addListener(Events.onRoundEnd, roundEnd);
        //int bitmask = (1 << (11 + plr));
        //gameObject.layer = bitmask;


    }
}
