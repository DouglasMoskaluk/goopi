using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int[] playersRoundsWonCount = new int[4];
    [SerializeField] private int[] playersTotalKillCount = new int[4];
    public int maxRoundsPlayed = 4;
    [SerializeField] private float displayEndTextLength = 3f;


    private void Awake()
    {
        if (instance == null) instance = this;

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        //EventManager.instance.addListener(Events.onGameEnd, ResetManager);
        RoundManager.instance.startRound();
        
    }

    public void ResetManager(EventParams par = new EventParams())
    {
        for (int i = 0; i < playersRoundsWonCount.Length; i++)
        {
            playersRoundsWonCount[i] = 0;
        }
        for (int i = 0; i < playersTotalKillCount.Length; i++)
        {
            playersTotalKillCount[i] = 0;
        }
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
    /// Procedure for what happens when the game ends
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndGame()
    {
        
        List<int> playerGameWinID = SelectGameWinners();
        string winnersString = "Player(s) ";
        foreach (int i in playerGameWinID)
        {
            winnersString += (i + 1).ToString() + " ";
        }
        winnersString += "won the game!";
        //yield return GameUIManager.instance.StartCoroutine(GameUIManager.instance.DisplayGameEndUI(displayEndTextLength, winnersString));
        //yield return SceneTransitionManager.instance.switchScene(Scenes.Podium);

        //yield return GameUIManager.instance.FadeIn(0.5f);

        yield return SceneTransitionManager.instance.loadScene(Scenes.Podium);

        //SplitScreenManager.instance.DisablePlayerControls();

        EventManager.instance.invokeEvent(Events.onGameEnd);

        yield return SceneTransitionManager.instance.unloadScene(Scenes.Arena);

        PodiumManager.instance.SetUpPodium(GetFinalGameData());

        AudioManager.instance.PlaySound(AudioManager.AudioQueue.WINNER);

        GunManager.instance.destroyParentedWorldObjects();
        SplitScreenManager.instance.DisableJoining();
        
        //PodiumManager.instance.SetScores(playersRoundsWonCount);
        PodiumManager.instance.SetWinnerText(winnersString);
        ResetManager();

        //yield return GameUIManager.instance.FadeOut(0.25f);
        float cutoutVisibleFadeTime = GameUIManager.instance.cutoutFadeToVisible();
        yield return new WaitForSecondsRealtime(cutoutVisibleFadeTime);

        PodiumManager.instance.StartPodiumSequence();
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

    private List<PlayerWinsData> GetFinalGameData()
    {

        List<PlayerWinsData> resultData = new List<PlayerWinsData>();

        for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            resultData.Add
                (
                    new PlayerWinsData(i, playersRoundsWonCount[i], playersTotalKillCount[i])
                );
        }

        resultData.Sort(new PlayerWinsDataKillsComparator());
        resultData.Sort(new PlayerWinsDataWinsComparator());

        return resultData;
    }

    public void ReadyArena()
    {
        StartCoroutine(LockerRoomToArenaTransition());
    }

    public void ReadyLockerRoom()
    {
        StartCoroutine(MMToLockerRoom());
    }

    private IEnumerator MMToLockerRoom()
    {
        AudioManager.instance.TransitionTrack("Locker Room");

        //yield return GameUIManager.instance.FadeIn(0.5f);
        float fadeBlackTime = GameUIManager.instance.cutoutFadeToBlack();
        yield return new WaitForSecondsRealtime(fadeBlackTime);

        yield return new WaitForSecondsRealtime(0.5f);

        GunManager.instance.SetLockerroomGuns();

        yield return SceneTransitionManager.instance.unloadScene(Scenes.MainMenu);
        GameUIManager.instance.cutoutFadeToVisibleInstant();
        yield return SceneTransitionManager.instance.loadScene(Scenes.LockerRoom);

    }

    private IEnumerator LockerRoomToArenaTransition()
    {
        //Debug.Log("inside ready singal");
        SplitScreenManager.instance.DisablePlayerControls();
        SplitScreenManager.instance.DisableJoining();

        AudioManager.instance.TransitionTrack("Roulette Spin");

        //yield return GameUIManager.instance.FadeIn(0.5f);
        yield return GameUIManager.instance.cutoutFadeToBlack();
        //Debug.Log("after fade");

        // ## why does this not execute after the other has finished ##
        yield return SceneTransitionManager.instance.unloadScene(Scenes.LockerRoom);
        //Debug.Log("after unload");

        yield return SceneTransitionManager.instance.loadScene(Scenes.Arena);
        //Debug.Log("after load ");

        StartGame();

        //yield return GameUIManager.instance.FadeOut(0.5f);
    }

    public int[] GetRoundsWon()
    {
        return playersRoundsWonCount;
    }
}

public class PlayerWinsData
{

    public int id;
    public int roundWins;
    public int totalKills;
    public int rank;
    public PlayerWinsData()
    {

    }

    public PlayerWinsData(int id, int roundWins, int totalKills)
    {
        this.id = id;
        this.roundWins = roundWins;
        this.totalKills = totalKills;
    }

    public PlayerWinsData(int id, int roundWins, int totalKills, int rank)
    {
        this.id = id;
        this.roundWins = roundWins;
        this.totalKills = totalKills;
        this.rank = rank;
    }

    public override string ToString()
    {
        return "Data: " + id + ", " + roundWins + ", " + totalKills + ", " + rank;
    }

}

//higher better
class PlayerWinsDataWinsComparator : IComparer<PlayerWinsData>
{
    public int Compare(PlayerWinsData x, PlayerWinsData y)
    {
        if (x.roundWins == y.roundWins)// the same
        {
            return 0;
        }
        else if (x.roundWins > y.roundWins)
        {
            return -1;
        }
        else
        {
            return 1;
        }

    }
}

class PlayerWinsDataKillsComparator : IComparer<PlayerWinsData>
{
    public int Compare(PlayerWinsData x, PlayerWinsData y)
    {
        if (x.totalKills == y.totalKills)// the same
        {
            return 0;
        }
        else if (x.totalKills > y.totalKills)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
