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
        
        if(SplitScreenManager.instance.GetPlayerCount() > 2)
        {
            screens[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-480, 270);
            screens[0].GetComponent<RectTransform>().sizeDelta = new Vector2(960, 540);

            screens[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(480, 270);
            screens[1].GetComponent<RectTransform>().sizeDelta = new Vector2(960, 540);
        }
        else
        {
            screens[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 270);
            screens[0].GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 540);

            screens[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -270);
            screens[1].GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 540);
        }

        screens[player].SetActive(true);

        //EventManager.instance.addListener(Events.onPlayerDeath, deathCheck);
        //EventManager.instance.addListener(Events.onEventEnd, roundEnd);
    }
}
