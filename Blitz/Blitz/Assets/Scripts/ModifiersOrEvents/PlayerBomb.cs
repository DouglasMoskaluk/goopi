using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField]
    private float timerStart = 10;
    private float timer = 10;
    [SerializeField]
    private int damage = 200;

    int plrID;

    private void Awake()
    {
        if (transform.parent.GetComponent<PlayerBodyFSM>() == null)
        {
            Destroy(this);
        }
        timer = timerStart;
        plrID = transform.parent.GetComponent<PlayerBodyFSM>().playerID;
        EventManager.instance.addListener(Events.onPlayerRespawn, resetTimer);
    }

    private void Start()
    {

        EventManager.instance.addListener(Events.onRoundEnd, roundEnd);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && transform.parent.GetComponent<CharacterController>().enabled == true)
        {
            transform.parent.GetComponent<PlayerBodyFSM>().damagePlayer(damage, -1);
        }
    }

    private void resetTimer(EventParams param = new EventParams())
    {
        if (plrID == param.killed) timer = timerStart;
    }

    private void roundEnd(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

}
