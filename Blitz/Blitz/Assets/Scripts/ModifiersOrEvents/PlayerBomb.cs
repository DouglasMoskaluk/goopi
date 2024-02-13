using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField]
    private float timerStart = 10;
    [SerializeField]
    private float timer = 29;
    [SerializeField]
    private float killTime = 5;
    [SerializeField]
    private int damage = 200;

    [SerializeField]
    TMP_Text countdownTimer;
    [SerializeField]
    GameObject explodeVFX;

    int plrID;

    private void Awake()
    {
        if (transform.parent.GetComponent<PlayerBodyFSM>() == null)
        {
            Destroy(this);
        }
        //timer = timerStart;
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
        if (timer < 4)
        {
            countdownTimer.color = Color.red;
        }
        else countdownTimer.color = Color.white;
        if (timer < 0 && transform.parent.GetComponent<CharacterController>().enabled == true)
        {
            transform.parent.GetComponent<PlayerBodyFSM>().damagePlayer(damage, -1);
            Instantiate(explodeVFX, transform.position, transform.rotation);
        }
        //transform.GetChild(0).transform.rotation = transform.parent.GetComponentInChildren<Camera>().transform.rotation;
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, transform.parent.GetComponentInChildren<Camera>().transform.eulerAngles.y, 0);
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
            if (timer > timerStart) timer = timerStart;
        }
    }

    private void roundEnd(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

}
