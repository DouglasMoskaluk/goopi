using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PodiumManager : MonoBehaviour
{
    public static PodiumManager instance;

    [SerializeField] private Slider[] playerScores;
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform podiumLocations;

    private void Awake()
    {
        if (instance == null) instance = this;
        SetUpPodium(new List<PlayerWinsData>()); //testing
    }

    public void SetUpPodium(List<PlayerWinsData> gameData)
    {
        

        //testing
        //gameData.Clear();
        //gameData.Add(new PlayerWinsData(0, 4, 16));
        //gameData.Add(new PlayerWinsData(1, 4, 13));
        //gameData.Add(new PlayerWinsData(3, 2, 4));
        //gameData.Add(new PlayerWinsData(2, 0, 7));

        FindPlayerRanks(ref gameData);

        PlacePlayers(gameData);

    }

    private void FindPlayerRanks(ref List<PlayerWinsData> gameData)
    {
        int nextRank = 0;
        for (int i = 0; i < 4; i++)
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
    }

    public void SetScores(int[] scores)
    {
        for (int i = 0; i < 4; i++)
        {
            playerScores[i].value = scores[i];
            playerScoreText[i].text = scores[i].ToString();
        }
    }

    public void PlacePlayers(List<PlayerWinsData> gameData)
    {
        for (int i = 0; i < 4; i++)
        {

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
