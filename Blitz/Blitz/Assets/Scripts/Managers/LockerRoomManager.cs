using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockerRoomManager : MonoBehaviour
{
    public static LockerRoomManager instance;

    [SerializeField] private GameObject[] joinTexts;

    public CharacterPiston[] roomPistons;

    private bool[] readyFlags;
    private bool readied = false;

    [SerializeField]
    private float shakeStrength;

    [SerializeField]
    private float shakeLength;

    private void Awake()
    {
        if (instance == null) instance = this;
        readyFlags = new bool[4];
        SplitScreenManager.instance.AllowJoining();
    }

    private void Start()
    {
        for(int i = 0; i < roomPistons.Length; i++)
        {
            roomPistons[i].shakeStrength = shakeStrength;
            roomPistons[i].shakeLength = shakeLength;

        }
    }

    private void Update()
    {
        //DEBUGGING PURPOSES
        if (Input.GetKeyDown(KeyCode.U))
        {
            SendReadySignal();
        }
    }

    private void AllPlayersReady()
    {
        SceneTransitionManager.instance.switchScene(Scenes.Arena);
    }

    public void DisableJoinText(int index)
    {
        joinTexts[index].SetActive(false);
    }

    public void InitializePlayerRoom(int index, GameObject player)
    {
        roomPistons[index].getPlayer(player);
    }

    public void ReadyUpPlayer(int playerID)
    {
        //Debug.Log("Player " + playerID + " has readied up.");
        readyFlags[playerID] = true;
        if (!readied && CheckReadyStatus())
        {
            readied = true;
            SendReadySignal();
            //SceneTransitionManager.instance.loadScene(Scenes.Arena);
        }
    }

    private void SendReadySignal()
    {
        GameManager.instance.ReadyArena();
    }

    private bool CheckReadyStatus()
    {
        for (int i = 0; i < 4; i++)
        {
            if (readyFlags[i] == false) return false;
        }
        return true;
    }


}
