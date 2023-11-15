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

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StartRound()
    {
        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            playerKillCounts[i] = 0;
        }

        elapsedTime = 0f;
        roundNum++;
    }

    public void EndRound()
    {
        List<int> winners = SelectRoundWinner();
        string winnerString = "Winners are PLayers: ";
        foreach (int w in winners)
        {
            winnerString += w.ToString() + ", ";
        }
        Debug.Log(winnerString);
        StartRound();
    }

    private void LateUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= roundLength)
        {
            EndRound();
        }
    }

    public float GetRoundTime()
    {
        return roundLength - elapsedTime;
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
}
