using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachedDeletion : MonoBehaviour
{
    int plr = -1;

    private void Start()
    {
        plr = transform.parent.GetComponent<PlayerBodyFSM>().playerID;
        EventManager.instance.addListener(Events.onPlayerDeath, deathCheck);
        EventManager.instance.addListener(Events.onRoundEnd, roundEnd);
    }


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
}
