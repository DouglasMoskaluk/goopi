using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockerRoomManager : MonoBehaviour
{
    public static LockerRoomManager instance;

    [SerializeField] private GameObject[] joinTexts;

    private bool[] readyFlags;
    private bool readied = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        readyFlags = new bool[4];
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
        Debug.Log("Player " + playerID + " has readied up.");
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
        StartCoroutine(ReadySignal());
    }

    private IEnumerator ReadySignal()
    {

        Debug.Log("inside ready singal");
        SplitScreenManager.instance.DisablePlayerControls();

        yield return GameUIManager.instance.FadeIn(0.5f);
        Debug.Log("after fade");

        // ## why does this not execute after the other has finished ##
        //yield return SceneTransitionManager.instance.unloadScene(Scenes.LockerRoom);
        Debug.Log("after unload");

        yield return SceneTransitionManager.instance.loadScene(Scenes.Arena);
        Debug.Log("after load ");

        GameManager.instance.StartGame();
    }

    private bool CheckReadyStatus()
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    if (readyFlags[i] == false) return false;
        //}
        return true;
    }


}
