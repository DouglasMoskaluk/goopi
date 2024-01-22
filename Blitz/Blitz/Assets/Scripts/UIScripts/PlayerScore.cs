using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private GameObject[] wins;
    private int winCount;


    public void SetWins(int amount)
    {
        winCount = amount;
        for (int i = 0; i < 5; i++)
        {
            if (i < amount) wins[i].SetActive(true);
            else wins[i].SetActive(false);
        }
    }

    public int GetWins()
    {
        return winCount;
    }
}
