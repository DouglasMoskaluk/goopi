using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    [SerializeField] private int roundNum = 0;
    [SerializeField] private float elapsedTime = 0.0f;
    [SerializeField] private float roundLength = 90f;
    [SerializeField] private int[] playerKillCounts = new int[4];
    [SerializeField] private float endRoundTextShownLength = 4f;

    public bool shouldCountDown = true;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StartRound()
    {
        RespawnManager.instance.RespawnAllPlayers();

        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            playerKillCounts[i] = 0;
        }

        roundNum++;
        shouldCountDown = true;
    }

    /// <summary>
    /// procedure for what happens when a round ends/is won
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndRound()
    {
        //figure out who won
        List<int> winners = SelectRoundWinner();//create list of round winners

        //create text string to say who won
        string winnerString = "Winners are Players: ";
        foreach (int w in winners)
        {
            winnerString += w.ToString() + ", ";
        }

        //display who won for a couple seconds
        yield return GameUIManager.instance.StartCoroutine(GameUIManager.instance.DisplayRoundEndUI(endRoundTextShownLength, winnerString));

        //cascade round information up to the game manager and let it decide what should be done next
        GameManager.instance.RoundWon(winners, playerKillCounts);
    }

    private void LateUpdate()
    {
        if (shouldCountDown)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= roundLength)
            {
                StartCoroutine(EndRound());
                elapsedTime = 0f;
                shouldCountDown = false;
            }
        }
        
    }

    public float GetRoundTime()
    {
        return roundLength - elapsedTime;
    }

    public void UpdateKillCount(int playerNum)
    {
        playerKillCounts[playerNum]++;
        
    }

    public int GetKillCount(int playerNum)
    {
        return playerKillCounts[playerNum];
    }

    private List<int> SelectRoundWinner()
    {
        List<int> result = new List<int>();

        int highest = -1;
        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            if (playerKillCounts[i] > highest)
            {
                highest = playerKillCounts[i];
            }
        }

        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            if (playerKillCounts[i] >= highest) { result.Add(i); }
        }

        return result;
    }

    public int GetRoundNum()
    {
        return roundNum;
    }
}
