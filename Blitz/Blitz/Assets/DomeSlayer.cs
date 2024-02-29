using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeSlayer : MonoBehaviour
{
    List<PlayerBodyFSM> players;
    private float deathDistance = 40;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<PlayerBodyFSM>();
        for (int i=0; i<SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            players.Add(SplitScreenManager.instance.GetPlayers(i));
        }
        EventManager.instance.addListener(Events.onPlayStart, onPlayStart);
    }

    void onPlayStart(EventParams param = new EventParams())
    {
        StartCoroutine(deathCheck());
    }

    
    IEnumerator deathCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < players.Count; i++)
            {
                Debug.Log(players[i].transform.position + " + " + transform.position + " = " + Vector3.Distance(players[i].transform.position, transform.position));
                if (Vector3.Distance(players[i].transform.position, transform.position) > deathDistance)
                {
                    Debug.Log("DIE!!!!");
                    players[i].damagePlayer(200, -1, Vector3.zero, Vector3.zero);
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
    }
}
