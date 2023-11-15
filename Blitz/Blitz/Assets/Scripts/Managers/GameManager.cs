using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int[] playersRoundsWonCount = new int[4];


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
    }

    public void StartGame()
    {

    }

    public void EndGame()
    {

    }


}
