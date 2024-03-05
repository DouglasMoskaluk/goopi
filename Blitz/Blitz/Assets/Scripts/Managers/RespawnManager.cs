using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public bool DISPLAY_DEBUG_MESSAGES = true;

    [Tooltip("The distance threshold for deeming a respawn location as eligible for selection by the respawn algorithm")]
    public float respawnThreshold = 120f;

    [Tooltip("Locations to respawn players in the arena")] private List<Transform> respawnLocations;

    [Tooltip("Locations to initially spawn players in the arena")] private List<Transform> initialRespawnLocations;

    [Tooltip("Locations to spawn players in the locker room")] private List<Transform> lockerRespawnLocations;

    [Tooltip("locations to respawn playes in the lava event")] private List<Transform> lavaEventRespawnLocations;



    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        respawnLocations = new List<Transform>(4);
        lockerRespawnLocations = new List<Transform>(4);
        initialRespawnLocations = new List<Transform>(4);
        lavaEventRespawnLocations = new List<Transform>(4);

        //goes through all children
        foreach (Transform child in transform.GetChild(0))
        {
            lockerRespawnLocations.Add(child);
            
        }

        foreach (Transform child in transform.GetChild(1))
        {
            initialRespawnLocations.Add(child);
        }

        foreach (Transform child in transform.GetChild(2))
        {
            
            respawnLocations.Add(child);
        }

        foreach (Transform child in transform.GetChild(3))
        {
            lavaEventRespawnLocations.Add(child);
        }

        //RoundManager.instance.onRoundReset.AddListener(respawnAllPlayers);
        EventManager.instance.addListener(Events.onRoundStart, respawnAllPlayers);
    }

    public void respawnAllPlayers(EventParams param = new EventParams())
    {
        //Debug.Log("respawning all players");
        int index = 0;
        foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        {
            CharacterController cc = player.transform.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.SetPositionAndRotation(respawnLocations[index].position, respawnLocations[index].rotation);
            cc.enabled = true;
            index++;
        }
    }

    /// <summary>
    /// procedure for returning what respawn location a player will respawn at
    /// </summary>
    /// <returns> the respan location </returns>
    public Transform getRespawnLocation()
    {
        List<Transform> eligibleRespawns = new List<Transform>();

        for (int i = 0; i < respawnLocations.Count; i++)
        {
            bool eligible = true;
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
                if (Vector3.Distance(player.transform.position, respawnLocations[i].position) >= respawnThreshold)
                {
                    eligible = false;   
                }
            }
            if (eligible) eligibleRespawns.Add(respawnLocations[i]);
        }

        if (eligibleRespawns.Count < 0) return respawnLocations[Random.Range(0, respawnLocations.Count)];

        return eligibleRespawns[Random.Range(0, eligibleRespawns.Count)];



        //List<Transform> eligibleRespawns = new List<Transform>();

        //for (int i = 0; i < respawnLocations.Count; i++)
        //{
        //    foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        //    {
        //        if (Vector3.Distance(player.transform.position, respawnLocations[i].position) >= respawnThreshold)
        //        {
        //            eligibleRespawns.Add(respawnLocations[i]);
        //        }
        //    }
        //}

        //if (eligibleRespawns.Count < 0) return respawnLocations[Random.Range(0, respawnLocations.Count)];

        //return eligibleRespawns[Random.Range(0, eligibleRespawns.Count)];


        /*List<Transform> eligibileSpawns = new List<Transform>(4);
        eligibileSpawns.AddRange(respawnLocations);
        for (int i = eligibileSpawns.Count -1; i >= 0; i--)
        {
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
                if (Vector3.Distance(eligibileSpawns[i].position, player.transform.position) <= respawnThreshold)
                {
                    eligibileSpawns.RemoveAt(i);
                }
            }
        }
        if (eligibileSpawns.Count <= 0) return respawnLocations[Random.Range(0, respawnLocations.Count)];

        return eligibileSpawns[Random.Range(0,eligibileSpawns.Count)];*/

        /*float[] scores = new float[respawnLocations.Count];//list of scores

        int index = 0;
        foreach (Transform location in respawnLocations)// go through each respawn, and sum the distances to each player
        {
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
                if (player.transform.GetComponent<PlayerBodyFSM>().Health <= 0) { continue; }
                scores[index] += Vector3.Distance(location.position, player.transform.position);
            }
            index++;
        }

        List<Transform> elibibleLocations = new List<Transform>();//list of possible respawns
        for (int i = 0; i < scores.Length; i++)//add respawn point to eligible if score is greater or equal to threshold
        {
            if (scores[i] >= respawnThreshold)
            {
                elibibleLocations.Add(respawnLocations[i]);
            }
        }

        if (elibibleLocations.Count < 1)//if no spawns are eligible, select random
        {
            return respawnLocations[Random.Range(0, respawnLocations.Count)];
        }

        int selected = Random.Range(0, elibibleLocations.Count);//selected a respawn

        if (DISPLAY_DEBUG_MESSAGES)
        {
            Debug.Log("RESPAWN MANAGER: " + elibibleLocations.Count + " eligible respawn locations were found.");
            Debug.Log("RESPAWN MANAGER: respawn at position " + elibibleLocations[selected].position + " was selected for respawn");
            Debug.Log("RESPAWN MANAGER: Eligible locations ");
            elibibleLocations.ForEach(x => Debug.Log(x.name));
        }

        return elibibleLocations[selected];//return selected respawn*/
    }

    public Transform getInitialSpawnLocation(int spawnIndex)
    {
        return initialRespawnLocations[spawnIndex];
    }

    public Transform getLockerRoomRespawnLocation(int playerID)
    {
        return lockerRespawnLocations[playerID];
    }

    public Transform getSpecificLocation(int num)
    {
        return respawnLocations[num];
    }

    public Transform getLavaSpawnLocation()
    {

        List<Transform> eligibleRespawns = new List<Transform>();

        for (int i = 0; i < lavaEventRespawnLocations.Count; i++)
        {
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
                if (Vector3.Distance(player.transform.position, lavaEventRespawnLocations[i].position) >= respawnThreshold)
                {
                    eligibleRespawns.Add(lavaEventRespawnLocations[i]);
                }
            }
        }

        if (eligibleRespawns.Count < 0) return lavaEventRespawnLocations[Random.Range(0, lavaEventRespawnLocations.Count)];

        return eligibleRespawns[Random.Range(0, eligibleRespawns.Count)];



        //List<Transform> eligibleRespawns = new List<Transform>();

        //for (int i = 0; i < lavaEventRespawnLocations.Count; i++)
        //{
        //    foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        //    {
        //        if (Vector3.Distance(player.transform.position, lavaEventRespawnLocations[i].position) >= respawnThreshold)
        //        {
        //            eligibleRespawns.Add(lavaEventRespawnLocations[i]);
        //        }
        //    }
        //}

        //if (eligibleRespawns.Count < 0) return lavaEventRespawnLocations[Random.Range(0, lavaEventRespawnLocations.Count)];

        //return eligibleRespawns[Random.Range(0, eligibleRespawns.Count)];

        //List<Transform> eligibileSpawns = new List<Transform>(4);
        //eligibileSpawns.AddRange(lavaEventRespawnLocations);
        //for (int i = eligibileSpawns.Count - 1; i >= 0; i--)
        //{
        //    foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        //    {
        //        if (Vector3.Distance(eligibileSpawns[i].position, player.transform.position) <= respawnThreshold)
        //        {
        //            eligibileSpawns.RemoveAt(i);
        //        }
        //    }
        //}
        //if (eligibileSpawns.Count <= 0) return lavaEventRespawnLocations[Random.Range(0, respawnLocations.Count)];

        //return eligibileSpawns[Random.Range(0, eligibileSpawns.Count)];


        /*float[] scores = new float[lavaEventRespawnLocations.Count];//list of scores

        int index = 0;
        foreach (Transform location in lavaEventRespawnLocations)// go through each respawn, and sum the distances to each player
        {
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
                if (player.transform.GetComponent<PlayerBodyFSM>().Health <= 0) { continue; }
                scores[index] += Vector3.Distance(location.position, player.transform.position);
            }
            index++;
        }

        List<Transform> elibibleLocations = new List<Transform>();//list of possible respawns
        for (int i = 0; i < scores.Length; i++)//add respawn point to eligible if score is greater or equal to threshold
        {
            if (scores[i] >= respawnThreshold)
            {
                elibibleLocations.Add(lavaEventRespawnLocations[i]);
            }
        }

        if (elibibleLocations.Count < 1)//if no spawns are eligible, select random
        {
            return lavaEventRespawnLocations[Random.Range(0, lavaEventRespawnLocations.Count)];
        }

        int selected = Random.Range(0, elibibleLocations.Count);//selected a respawn

        if (DISPLAY_DEBUG_MESSAGES)
        {
            Debug.Log("RESPAWN MANAGER: " + elibibleLocations.Count + " eligible respawn locations were found.");
            Debug.Log("RESPAWN MANAGER: respawn at position " + elibibleLocations[selected].position + " was selected for respawn");
            Debug.Log("RESPAWN MANAGER: Eligible locations ");
            elibibleLocations.ForEach(x => Debug.Log(x.name));
        }

        return elibibleLocations[selected];//return selected respawn*/
    }
}
