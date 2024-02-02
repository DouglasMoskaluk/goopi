using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PodiumManager : MonoBehaviour
{
    public static PodiumManager instance;

    [SerializeField] private Slider[] playerScores;
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform podiumLocations;
    private List<Transform> podiumPositions;
    [SerializeField] private Transform podiumLookTransform;

    private void Awake()
    {
        if (instance == null) instance = this;
        podiumPositions = new List<Transform>(4);
        foreach (Transform child in podiumLocations)
        {
            podiumPositions.Add(child); 
        }
        //SetUpPodium(new List<PlayerWinsData>()); //testing
    }

    public void SetUpPodium(List<PlayerWinsData> gameData)
    {


        //testing
        //gameData.Clear();
        //gameData.Add(new PlayerWinsData(0, 4, 16));
        //gameData.Add(new PlayerWinsData(1, 4, 13));
        //gameData.Add(new PlayerWinsData(3, 2, 4));
        //gameData.Add(new PlayerWinsData(2, 0, 7));

        gameData = FindPlayerRanks(gameData);

        PlacePlayersOnPodium(gameData);

        

    }

    private List<PlayerWinsData> FindPlayerRanks(List<PlayerWinsData> gameData)
    {
        int nextRank = 0;
        for (int i = 0; i < gameData.Count; i++)
        {
            if (i == 3) { gameData[i].rank = nextRank; break; }

            if (gameData[i].roundWins == gameData[i + 1].roundWins)
            {
                gameData[i].rank = nextRank;
            }
            else
            {
                gameData[i].rank = nextRank++;
            }
        }
        return gameData;
    }

    public void SetScores(int[] scores)
    {
        for (int i = 0; i < 4; i++)
        {
            playerScores[i].value = scores[i];
            playerScoreText[i].text = scores[i].ToString();
        }
    }

    public void PlacePlayersOnPodium(List<PlayerWinsData> gameData)
    {
        List<PlayerInput> players = SplitScreenManager.instance.GetPlayers();

        for (int i = 0; i < gameData.Count; i++)
        {
            
            CharacterController chara = players[gameData[i].id].transform.GetComponent<CharacterController>();
            PlayerBodyFSM FSM = players[gameData[i].id].transform.GetComponent<PlayerBodyFSM>();
            chara.enabled = false;
            players[gameData[i].id].transform.position = podiumPositions[i].position;
            players[gameData[i].id].transform.rotation = podiumPositions[i].rotation;

            FSM.SetCameraLookAt(podiumLookTransform);
            FSM.SetBodyRotToCamera();
            FSM.DisablePlayerCamera();
            FSM.transitionState(PlayerMotionStates.Walk);
            FSM.transitionState(PlayerActionStates.Idle);



            chara.enabled = true;

        }
    }

    public void SetWinnerText(string text)
    {
        winnerText.text = text;
    }

    public void OnExitButtonPressed()
    {
        AudioManager.instance.TransitionTrack("MainMenu");
        SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
    }
}
