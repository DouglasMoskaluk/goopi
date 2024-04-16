using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
#if UNITY_EDITOR
using TMPro.EditorUtilities;
#endif

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


    private void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, ResetCrowns);
        //EventManager.instance.addListener(Events.onPlayerDeath, SetCrownsEventUsage, 3);
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

        //GunManager.instance.assignGun(players.Count - 1);

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

        PlayerBodyFSM fsm = player.transform.GetComponent<PlayerBodyFSM>();
        fsm.playerID = players.Count - 1;
        fsm.SetGrenadeArcRendererLayer(layerToAdd);

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
        LockerRoomManager.instance.InitializePlayerRoom(players.Count - 1, player.transform.gameObject);

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

    public void EnablePlayerControlsByID(int ID)
    {
        players[ID].ActivateInput();
    }

    public void DisablePlayerControlsByID(int ID)
    {
        players[ID].currentActionMap.Disable();
    }

    public void DisablePlayerControls()
    {
        for (int i = 0; i < players.Count; i++)
        {
            //players[i].DeactivateInput();
            players[i].currentActionMap.Disable();
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

    public PlayerInput GetPlayerByID(int id)
    {
        return players[id];
    }

    public void SetCrowns()//again ik this is not the correct spot to do it but i dont have time to rework the events enough to do it with them rn
    {

        if (RoundManager.instance.getHighestKillsNumber() == 0) return;

        List<int> highestRoundKills = RoundManager.instance.GetHighestRoundKills();

        for (int i = 0; i < players.Count; i++)
        {
            PlayerBodyFSM FSM = players[i].GetComponent<PlayerBodyFSM>();
            //bool shouldBeVisible = highestRoundKills.Contains(i);
            FSM.SetCrownVisibility(highestRoundKills.Contains(i));
        }
    }

    public void SetCrownsEventUsage(EventParams param = new EventParams())
    {
        SetCrowns();
    }

    public void ResetCrowns(EventParams param = new EventParams()) // same thing with above
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerBodyFSM>().SetCrownVisibility(false); ;
        }
    }
}
