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
            player.transform.SetPositionAndRotation(initialRespawnLocations[index].position, initialRespawnLocations[index].rotation);
            player.GetComponent<PlayerBodyFSM>().RotateCameraTo(initialRespawnLocations[index].GetComponent<RespawnPointPlayerRotationValueHolder>());
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
        
        List<Transform> eligibileSpawns = new List<Transform>(4);
        eligibileSpawns.AddRange(respawnLocations);
        for (int i = eligibileSpawns.Count - 1; i >= 0; i--)
        {
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
                if (Vector3.Distance(eligibileSpawns[i].position, player.transform.position) <= respawnThreshold)
                {
                    eligibileSpawns.RemoveAt(i);
                    break;
                }
            }
        }
        if (eligibileSpawns.Count <= 0) return respawnLocations[Random.Range(0, respawnLocations.Count)];

        return eligibileSpawns[Random.Range(0, eligibileSpawns.Count)];
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

        List<Transform> eligibileSpawns = new List<Transform>(4);
        eligibileSpawns.AddRange(lavaEventRespawnLocations);
        for (int i = eligibileSpawns.Count - 1; i >= 0; i--)
        {
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
                
                if (player.GetComponent<PlayerBodyFSM>().deathCheck)
                {
                    continue;
                }

                if (Vector3.Distance(eligibileSpawns[i].position, player.transform.position) <= respawnThreshold)
                {
                    eligibileSpawns.RemoveAt(i);
                    break;
                }
            }
        }

        //eligibileSpawns.ForEach(spawn => Debug.Log(spawn.name));

        if (eligibileSpawns.Count <= 0) return lavaEventRespawnLocations[Random.Range(0, lavaEventRespawnLocations.Count)];

        return eligibileSpawns[Random.Range(0, eligibileSpawns.Count)];
    }
}
