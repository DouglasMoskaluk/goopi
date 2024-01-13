using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    //public UnityEvent onRoundReset;

    [SerializeField] private int roundNum = 0;
    [SerializeField] private float elapsedTime = 0.0f;
    [SerializeField] private float roundLength = 90f;
    [SerializeField] private int[] playerKillCounts = new int[4];
    [SerializeField] private float endRoundTextShownLength = 4f;

    public bool shouldCountDown = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        roundLength += 0.99f;//so that it stays on the desired start number for 1 second instead of instantly going down
    }

    public Coroutine startRound()
    {
        return StartCoroutine(startRoundCoro());
    }

    private IEnumerator startRoundCoro()
    {

        EventManager.instance.invokeEvent(Events.onRoundStart);

        if (roundNum > 0) yield return GameUIManager.instance.FadeOut(0.25f);//if this is not the first round

        Time.timeScale = 1.0f;

        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            playerKillCounts[i] = 0;
        }

        roundNum++;
        shouldCountDown = true;

        SplitScreenManager.instance.EnablePlayerControls();
    }

    private IEnumerator endRoundCoro()
    {

        //const slow speed, start ts = 0.5, slow for 2s then fade over 0.5s

        Time.timeScale = 0.25f;
        //GameUIManager.instance.SetFadePanelAlpha(0.5f); //<- might want to have it fade to like 0.25 or 0.3 tp indicate the slowmo more

        yield return new WaitForSeconds(0.75f);

        yield return GameUIManager.instance.FadeIn(0.5f);

        SplitScreenManager.instance.DisablePlayerControls();
        Time.timeScale = 1f;

        EventManager.instance.invokeEvent(Events.onRoundEnd);
        shouldCountDown = false;
        //figure out who won
        List<int> winners = selectRoundWinner();//create list of round winners

        //create text string to say who won
        string winnerString = "Winners are Players: ";
        foreach (int w in winners)
        {
            winnerString += w.ToString() + ", ";
        }

        //display who won for a couple seconds
        yield return GameUIManager.instance.StartCoroutine(GameUIManager.instance.DisplayRoundEndUI(endRoundTextShownLength, winnerString));

        //cascade round information up to the game manager and let it decide what should be done next
        GameManager.instance.roundWon(winners, playerKillCounts);
    }

    /// <summary>
    /// procedure for what happens when a round ends/is won
    /// </summary>
    /// <returns></returns>
    public Coroutine endRound()
    {
        return StartCoroutine(endRoundCoro());
    }

    private void LateUpdate()
    {
        if (shouldCountDown)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= roundLength)
            {
                endRound();
                elapsedTime = 0f;
                shouldCountDown = false;
            }
        }
        
    }

    public float getRoundTime()
    {
        return roundLength - elapsedTime;
    }

    public void updateKillCount(int playerNum)
    {
        playerKillCounts[playerNum]++;
        SplitScreenManager.instance.GetPlayers(playerNum).playerUI.playerGotKill();
    }

    public int getKillCount(int playerNum)
    {
        return playerKillCounts[playerNum];
    }

    private List<int> selectRoundWinner()
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

    public int getRoundNum()
    {
        return roundNum;
    }
}
