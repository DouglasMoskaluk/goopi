using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField]
    private float timerStart = 10;
    private float timer = 10;
    [SerializeField]
    private float killTime = 5;
    [SerializeField]
    private int damage = 200;

    [SerializeField]
    TMP_Text countdownTimer;

    int plrID;

    private void Awake()
    {
        if (transform.parent.GetComponent<PlayerBodyFSM>() == null)
        {
            Destroy(this);
        }
        timer = timerStart;
        plrID = transform.parent.GetComponent<PlayerBodyFSM>().playerID;
        EventManager.instance.addListener(Events.onPlayerRespawn, ownerDied);
        EventManager.instance.addListener(Events.onPlayerDeath, playerDied);
    }

    private void Start()
    {

        EventManager.instance.addListener(Events.onRoundEnd, roundEnd);
    }

    private void Update()
    {
        if (timer >= -1) countdownTimer.text = "" + (int)Mathf.Floor(timer+1);
        timer -= Time.deltaTime;
        if (timer < 0 && transform.parent.GetComponent<CharacterController>().enabled == true)
        {
            transform.parent.GetComponent<PlayerBodyFSM>().damagePlayer(damage, -1);
        }
    }

    private void ownerDied(EventParams param = new EventParams())
    {
        if (plrID == param.killed) timer = timerStart;
    }

    private void playerDied(EventParams param = new EventParams())
    {
        if (plrID == param.killer && plrID != param.killed)
        {
            timer += killTime;
        }
    }

    private void roundEnd(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

}
