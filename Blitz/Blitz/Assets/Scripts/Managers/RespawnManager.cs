using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    private List<Transform> respawnLocations;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        respawnLocations = new List<Transform>(4);
        //goes through all children
        foreach (Transform child in transform)
        {
            respawnLocations.Add(child);
        }
        RoundManager.instance.onRoundReset.AddListener(respawnAllPlayers);
    }

    /// <summary>
    /// Selects which of the respawn locations is to be selected from respawnLocations, based on distance algorithm
    /// </summary>
    /// <returns> the integer index of the selected respawn location based on the respawnLocations array </returns>
    private int selectRespawnLocation()
    {
        //for now just picks a random one
        return Random.Range(0, respawnLocations.Count);
    }

    public void respawnAllPlayers()
    {
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
        return respawnLocations[selectRespawnLocation()];
    }

    public Transform getSpecificLocation(int num)
    {
        return respawnLocations[num];
    }
}
