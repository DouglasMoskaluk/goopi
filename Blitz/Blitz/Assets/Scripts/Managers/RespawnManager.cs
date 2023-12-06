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
        float[] scores = new float[respawnLocations.Count];//list of scores

        int index = 0;
        foreach (Transform location in respawnLocations)// go through each respawn, and sum the distances to each player
        {
            foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
            {
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
        }

        return elibibleLocations[selected];//return selected respawn
    }

    public Transform getSpecificLocation(int num)
    {
        return respawnLocations[num];
    }
}
