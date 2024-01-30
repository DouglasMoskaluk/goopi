using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SplitScreenManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();

    [SerializeField]
    private List<LayerMask> playerLayers;

    [SerializeField]
    private List<LayerMask> playerDotLayers;

    private PlayerInputManager inputManager;

    public static SplitScreenManager instance;

    private void Awake()
    {
        if (instance == null) instance = this; 
        inputManager = FindObjectOfType<PlayerInputManager>();
        inputManager.onPlayerJoined += AddPlayer;

    }

    public int GetPlayerCount()
    {
        return players.Count;
    }

    public void AllowJoining()
    {
        inputManager.EnableJoining();
    }

    public void DisableJoining()
    {
        inputManager.DisableJoining();
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        GunManager.instance.assignGun(players.Count - 1);

        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);

        //if (players.Count >= 4)
        //{
        //    RespawnManager.instance.RespawnAllPlayers();
        //}
        CharacterController cController = player.transform.GetComponent<CharacterController>();
        cController.enabled = false;

        Transform spawnPoint = RespawnManager.instance.getLockerRoomRespawnLocation(players.Count - 1);//spawn players at locker room locations because they will all join inside the locker room

        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        cController.enabled = true;

        player.transform.GetComponent<PlayerBodyFSM>().playerID = players.Count - 1;

        player.transform.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        player.transform.gameObject.layer = layerToAdd;
        foreach (Transform child in player.transform)
        {
            child.gameObject.layer = layerToAdd;
        }

        for (int i = 0; i < playerDotLayers.Count; i++)
        {
            int playerDotLayerToAdd = (int)Mathf.Log(playerDotLayers[i].value, 2);

            if (i == players.Count - 1)
            {
                player.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.layer = playerDotLayerToAdd;
            }
            else
            {
                player.transform.GetChild(0).GetChild(0).GetComponent<Camera>().cullingMask |= 1 << playerDotLayerToAdd;
            }
        }

        //add the layer
        player.transform.GetChild(0).GetChild(0).GetComponent<Camera>().cullingMask |= 1 << layerToAdd;

        player.transform.GetComponentInChildren<PlayerCamInput>().lookValue = player.actions.FindAction("Look");

        LockerRoomManager.instance.DisableJoinText(players.Count - 1);
        LockerRoomManager.instance.InitializePlayerRoom(players.Count - 1, player.transform.GetComponent<CameraShake>());

    }

    public int getPlayerID(GameObject player)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (player.GetComponent<PlayerInput>() == players[i])
            {
                return i;
            }
        }
        return -1;
    }

    public List<PlayerInput> GetPlayers()
    {
        return players;
    }

    public PlayerBodyFSM GetPlayers(int i)
    {
        return players[i].GetComponent<PlayerBodyFSM>();
    }

    public void DisablePlayers()
    {
        for (int i = 0;i < players.Count; i++)
        {
            players[i].gameObject.SetActive(false);
        }
    }

    public void EnablePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].gameObject.SetActive(true);
        }
    }

    public void EnablePlayerControls()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].ActivateInput();
            
        }
    }

    public void DisablePlayerControls()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].DeactivateInput();
        }
    }

    public void RemoveAllPlayers()
    {
        for (int i = players.Count - 1; i >= 0; i--)
        {
            Destroy(players[i].gameObject);
            players.RemoveAt(i);
        }
    }

    public void RemovePlayer(int index)
    {
        Destroy(players[index].gameObject);
        players.RemoveAt(index);
    }
}
