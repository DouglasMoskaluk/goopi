using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int[] playersRoundsWonCount = new int[4];
    [SerializeField] private int[] playersTotalKillCount = new int[4];
    public int maxRoundsPlayed = 5;
    [SerializeField] private float displayEndTextLength = 3f;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        //RoundManager.instance.startRound();
    }

    /// <summary>
    /// procedure for when a round is won/ends on the game manager side
    /// </summary>
    /// <param name="playerWonID"></param>
    /// <param name="kills"></param>
    public void roundWon(List<int> playerWonID, int[] kills)
    {
        //add which players won the round to the rounds won tacker
        foreach (int i in playerWonID)
        {
            playersRoundsWonCount[i]++;
        }
        //update players total kills
        UpdateTotalKills(kills);

        //determine if another round should be played, or if should end the game
        if (RoundManager.instance.getRoundNum() >= maxRoundsPlayed)
        {
            StartCoroutine(EndGame());//end the game
        }
        else
        {
            RoundManager.instance.startRound();//start another round
        }
    }

    /// <summary>
    /// procedure for when the game starts, game being the actual shooting part
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartGame()
    {
        yield return null;
    }

    /// <summary>
    /// Procedure for what happens when the game ends
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndGame()
    {
        List<int> playerGameWinID = SelectGameWinners();
        string winnersString = "Players ";
        foreach (int i in playerGameWinID)
        {
            winnersString += i.ToString() + " ";
        }
        winnersString += "won the game!";
        yield return GameUIManager.instance.StartCoroutine(GameUIManager.instance.DisplayGameEndUI(displayEndTextLength, winnersString));
    }

    public void UpdateTotalKills(int[] kills)
    {
        for (int i = 0; i < kills.Length; i++)
        {
            playersTotalKillCount[i] += kills[i];
        }
    }

    private List<int> SelectGameWinners()
    {
        List<int> result = new List<int>(4);

        //get highest rounds won
        int highest = 0;
        for (int i = 0; i < playersRoundsWonCount.Length; i++)
        {
            if (playersRoundsWonCount[i] >= highest)
            {
                highest = playersRoundsWonCount[i];
            }
        }

        //put all players with highest round wins in list
        for (int i = 0; i < playersRoundsWonCount.Length; i++)
        {
            if (playersRoundsWonCount[i] >= highest)
            {
                result.Add(i);
            }
        }

        //resolve draws
        if (result.Count > 1)
        {
            //get the highest kills
            int highestKills = 0;
            foreach (int i in result)
            {
                if (playersTotalKillCount[i] >= highestKills)
                {
                    highestKills = playersTotalKillCount[i];
                }
            }

            //remove players that don't have highest kills
            for (int i = result.Count-1; i >= 0; i--)
            {
                if (playersTotalKillCount[result[i]] < highestKills)
                {
                    result.Remove(i);
                }
            }
        }


        return result;
    }

}
