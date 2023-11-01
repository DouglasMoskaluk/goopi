using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// procedure for returning what respawn location a player will respawn at
    /// </summary>
    /// <returns> the respan location </returns>
    public Transform getRespawnLocation()
    {
        return respawnLocations[selectRespawnLocation()];
    }
}
