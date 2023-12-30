using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockerRoomManager : MonoBehaviour
{
    private void Awake()
    {
        SplitScreenManager.instance.AllowJoining();
    }

    private void AllPlayersReady()
    {
        SceneTransitionManager.instance.switchScene(Scenes.Arena);
    }
}
