using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int[] playersRoundsWonCount = new int[4];
    private int[] playersTotalKillCount = new int[4];
    public int maxRoundsPlayed = 5;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RoundWon(List<int> playerWonID)
    {
        foreach (int i in playerWonID)
        {
            playersRoundsWonCount[i]++;
        }
        if (RoundManager.instance.GetRoundNum() >= maxRoundsPlayed)
        {
            EndGame();
        }
    }

    public void StartGame()
    {

    }

    public void EndGame()
    {
        //SelectGameWinner;
    }

    public void UpdateTotalKills(int[] kills)
    {
        for (int i = 0; i < kills.Length; i++)
        {
            playersTotalKillCount[i] += kills[i];
        }
    }


}
