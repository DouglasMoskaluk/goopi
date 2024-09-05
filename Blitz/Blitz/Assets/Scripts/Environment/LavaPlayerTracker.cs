using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class LavaPlayerTracker : MonoBehaviour
{
    // Start is called before the first frame update

    private bool[] playerTouchLava;

    void OnEnable()
    {
        playerTouchLava = new bool[SplitScreenManager.instance.GetPlayerCount()];
        EventManager.instance.addListener(Events.onRoundEnd, CheckPlayers);
    }

    private void OnDisable()
    {
        EventManager.instance.removeListener(Events.onRoundEnd, CheckPlayers);
    }

    public void CheckPlayers(EventParams param = new EventParams())
    {
        for(int i = 0; i < playerTouchLava.Length;i++)
        {
            if (!playerTouchLava[i])
            {
                NewSteamManager.instance.UnlockAchievement(eAchivement.Lava);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //touched lava
            int id = SplitScreenManager.instance.getPlayerID(other.gameObject);
            playerTouchLava[id] = true;
        }
    }
}
