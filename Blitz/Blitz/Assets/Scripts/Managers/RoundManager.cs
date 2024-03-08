using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    public void Start()
    {
        EventManager.instance.addListener(Events.onGameEnd, ResetManager);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            roundNum = 4;
        }
    }

    public void ResetManager(EventParams par = new EventParams())
    {
        roundNum = 0;
        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            playerKillCounts[i] = 0;
        }
        shouldCountDown = false;
    }

    public Coroutine startRound()
    {
        return StartCoroutine(startRoundCoro());
    }

    private IEnumerator startRoundCoro()
    {
        
        foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        {
            PlayerBodyFSM FSM = player.GetComponent<PlayerBodyFSM>();
            FSM.SetPlayerSpineValue(0.5f);
            FSM.ForceAnimatorUpdate();
            
        }

        EventManager.instance.invokeEvent(Events.onRoundStart);

        yield return new WaitForSecondsRealtime(0.5f);

        //round ui stuff
        GameUIManager.instance.showRoundTransition();

        yield return GameUIManager.instance.spinGunSelection(GunManager.instance.GunUsed);

        float transitionMotionAnimTime = GameUIManager.instance.playGunTutorialMotion();

        yield return new WaitForSecondsRealtime(transitionMotionAnimTime);

        float gunTutAnimTime = GameUIManager.instance.playGunTutorial();

        yield return new WaitForSecondsRealtime(gunTutAnimTime);

        //yield return new WaitForSecondsRealtime(1.5f);


        GameUIManager.instance.hideRoundTransition();

        int playedEventAudio = 0;
        for (int i = 0; i < ModifierManager.instance.ActiveEvents.Length; i++)
        {
            if (ModifierManager.instance.ActiveEvents[i])
            {
                if (i == (int)ModifierManager.RoundModifierList.RICOCHET)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_RICOCHET, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.LOW_GRAVITY)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_LOWGRAV, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.RANDOM_GUNS)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_MEGA, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.FLOOR_IS_LAVA)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_LAVA, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.BOMB)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_BOMB, playedEventAudio * 2);
                }
                playedEventAudio++;
            }
        }
        ModifierManager.instance.showModifierUI();

        yield return GameUIManager.instance.FadeOut(0.75f);
        GameUIManager.instance.ResetRoundTransitionUI();

        Time.timeScale = 1.0f;

        //Turn camera here @Patrick
        if (ModifierManager.instance.getNumEvents(RoundManager.instance.roundNum - 1) > 0) yield return new WaitForSeconds(5f);

        ModifierManager.instance.hideModifierUI();

        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            playerKillCounts[i] = 0;
        }

        roundNum++;
        shouldCountDown = true;

        SplitScreenManager.instance.EnablePlayerControls();

        EventManager.instance.invokeEvent(Events.onPlayStart);

        Debug.Log("start Round co ends");
    }

    private IEnumerator endRoundCoro()
    {

        Debug.Log("end round co starts");

        //const slow speed, start ts = 0.5, slow for 2s then fade over 0.5s

        List<int> winners = selectRoundWinner();//create list of round winners

        AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROUND_VICTORY);

        float averageKills = 0;
        for (int i = 0; i < 4; i++)
        {
            averageKills += playerKillCounts[i];
        }
        averageKills /= 4;

        Debug.Log("Average kills for round " + roundNum + ": " + averageKills);

        Time.timeScale = 0.25f;
        //GameUIManager.instance.SetFadePanelAlpha(0.5f); //<- might want to have it fade to like 0.25 or 0.3 tp indicate the slowmo more
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROUND_END);
        yield return new WaitForSecondsRealtime(0.75f);

        yield return GameUIManager.instance.FadeIn(0.5f);
        yield return new WaitForSecondsRealtime(0.5f);

        SplitScreenManager.instance.DisablePlayerControls();
        Time.timeScale = 1f;

        EventManager.instance.invokeEvent(Events.onRoundEnd);
        shouldCountDown = false;
        //figure out who won

        //create text string to say who won
        string winnerString = "Winners are Players: ";
        foreach (int w in winners)
        {
            winnerString += w.ToString() + ", ";
        }

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

        for (int i=0; i<result.Count && i < SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            SplitScreenManager.instance.GetPlayers(result[i]).playerUI.showVictoryText();
        }
        return result;
    }

    public int getRoundNum()
    {
        return roundNum;
    }

    public List<int> GetHighestRoundKills()
    {
        List<int> result = new List<int>(); 
        int highest = 0;
        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            if (playerKillCounts[i] > highest) highest = playerKillCounts[i];
        }

        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            if (playerKillCounts[i] >= highest) result.Add(i);
        }
        return result;
    }

    public int getHighestKillsNumber()
    {
        int highest = 0;
        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            if (playerKillCounts[i] > highest) highest = playerKillCounts[i]; 
        }
        return highest;
    }
}
