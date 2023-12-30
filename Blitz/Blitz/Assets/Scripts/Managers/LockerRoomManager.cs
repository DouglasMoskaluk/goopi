using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockerRoomManager : MonoBehaviour
{
    public static LockerRoomManager instance;

    [SerializeField] private GameObject[] joinTexts;

    private bool[] readyFlags;

    private void Awake()
    {
        if (instance == null) instance = this;
        SplitScreenManager.instance.AllowJoining();
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

    public void ReadyUpPlayer(int playerID)
    {
        readyFlags[playerID] = true;
        if (CheckReadyStatus())
        {
            SendReadySignal();
        }
    }

    private void SendReadySignal()
    {
        SceneTransitionManager.instance.switchScene(Scenes.Arena);
        GameManager.instance.StartGame();
    }

    private bool CheckReadyStatus()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!readyFlags[i]) return false;
        }
        return true;
    }


}
