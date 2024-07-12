using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    //public UnityEvent onRoundReset;

    [SerializeField] private int roundNum = 0;
    [SerializeField] private float elapsedTime = 0.0f;
    [SerializeField] private float roundLength = 90f;
    [SerializeField] private int[] playerKillCounts = new int[4];
    [SerializeField] private float endRoundTextShownLength = 4f;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile postProfile;

    private DepthOfField dof;

    [SerializeField]
    private Color baseFilter;

    [SerializeField]
    private Color endOfRoundFilter;

    [SerializeField]
    private Color lowGravFilter;

    private ColorAdjustments colorAdj;

    private float blurEndPoint = 30f;

    public bool shouldCountDown = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        roundLength += 0.99f;//so that it stays on the desired start number for 1 second instead of instantly going down
    }

    public void Start()
    {
        EventManager.instance.addListener(Events.onGameEnd, ResetManager);
        ResetPostProcess();
    }

    public void Update()
    {
        if (GameManager.instance.ALLOW_KEYBOARD_DEVKEYS && Input.GetKey(KeyCode.N))
        {
            roundNum = 4;
        }
    }

    public void activateJudgeMode()
    {
        roundNum = 5;
    }

    public void ResetManager(EventParams par = new EventParams())
    {
        roundNum = 0;
        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            playerKillCounts[i] = 0;
        }
        elapsedTime = 0;
        shouldCountDown = false;
    }

    public Coroutine startRound()
    {
        return StartCoroutine(startRoundCoro());
    }

    private IEnumerator startRoundCoro()
    {
        GameUIManager.instance.SetRoundDisplayString();
        foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        {
            PlayerBodyFSM FSM = player.GetComponent<PlayerBodyFSM>();
            FSM.SetPlayerSpineValue(0.5f);
            FSM.ForceAnimatorUpdate();
        }

        EventManager.instance.invokeEvent(Events.onRoundStart);

        yield return new WaitForSecondsRealtime(0.5f);

        GameUIManager.instance.setTutAnimInitFrame();
        GameUIManager.instance.ResetSpinner();

        //round ui stuff
        GameUIManager.instance.showRoundTransition();

        float cutoutFadeVisible = GameUIManager.instance.cutoutFadeToVisible();
        yield return new WaitForSecondsRealtime(cutoutFadeVisible);


        yield return GameUIManager.instance.spinGunSelection(GunManager.instance.GunUsed);

        if (roundNum != 3 && roundNum != 7)//round 4
        {
            float transitionMotionAnimTime = GameUIManager.instance.playGunTutorialMotion();
            yield return new WaitForSecondsRealtime(transitionMotionAnimTime);


            float gunTutAnimTime = GameUIManager.instance.playGunTutorial();
            yield return new WaitForSecondsRealtime(gunTutAnimTime);
        }

        if (roundNum != 0)
        {
            ModifierManager.instance.showModifierUI();
            yield return new WaitForSecondsRealtime(5.5f);
            if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RANDOM_GUNS]) yield return new WaitForSeconds(2.92f);
            GameUIManager.instance.cutoutFadeToBlackInstant();
            ModifierManager.instance.hideModifierUI();
        } else
        {
            float fadeToBlack = GameUIManager.instance.cutoutFadeToBlack();
            yield return new WaitForSecondsRealtime(fadeToBlack);
        }


        yield return new WaitForSecondsRealtime(0.5f);//stay on black screen for 1.5s


        GameUIManager.instance.hideRoundTransition();

        

        GameUIManager.instance.ResetRoundTransitionUI();
        Time.timeScale = 1.0f;


        for (int i = 0; i < playerKillCounts.Length; i++)
        {
            playerKillCounts[i] = 0;
        }

        roundNum++;
        shouldCountDown = true;

        SplitScreenManager.instance.EnablePlayerControls();

        EventManager.instance.invokeEvent(Events.onPlayStart);

        foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        {
            player.GetComponent<PlayerBodyFSM>().EnablePlayerCamera();
        }

        float secondCutoutFadeVisible = GameUIManager.instance.cutoutFadeToVisible();
        yield return new WaitForSecondsRealtime(secondCutoutFadeVisible);

        //Debug.Log("start Round co ends");
    }

    private IEnumerator blurEffectCoro()
    {
        float timer = 0;
        float blurPoint = 1f;

        Color lerpColor;

        Color startColor;

        if(ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.LOW_GRAVITY])
        {
            startColor = lowGravFilter;
        }
        else
        {
            startColor = baseFilter;
        }


        //dof.focalLength.Override(blurEndPoint);

        while (timer < 0.5f)
        {
            timer += Time.fixedDeltaTime;

            float ratio = timer / 1f;

            blurPoint = Mathf.Lerp(1f, blurEndPoint, ratio);
            lerpColor = Color.Lerp(startColor, endOfRoundFilter, ratio);

            colorAdj.colorFilter.Override(lerpColor);
            dof.focalLength.Override(blurPoint);

            yield return null;
        }


    }


    private IEnumerator endRoundCoro()
    {


        //Debug.Log("end round co starts");

        //const slow speed, start ts = 0.5, slow for 2s then fade over 0.5s

        List<int> winners = selectRoundWinner();//create list of round winners

        //remove player UI
        for(int i = 0; i < SplitScreenManager.instance.GetPlayerCount();i++)
        {
            PlayerBodyFSM newPlayer = SplitScreenManager.instance.GetPlayers(i);
            newPlayer.playerUI.scaleObject.SetActive(false);
        }

        GameUIManager.instance.HideTimerObject();
        GameUIManager.instance.StartCrownSequence();

        StartCoroutine(blurEffectCoro());
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROUND_VICTORY);

        float averageKills = 0;
        for (int i = 0; i < 4; i++)
        {
            averageKills += playerKillCounts[i];
        }
        averageKills /= 4;

        //Debug.Log("Average kills for round " + roundNum + ": " + averageKills);

        Time.timeScale = 0.25f;

        AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROUND_END);

        yield return new WaitForSecondsRealtime(0.85f);

        GameUIManager.instance.RoundVictoryCrownFly(winners);


        yield return new WaitForSecondsRealtime(2.5f);

        float cutoutFadeBlack = GameUIManager.instance.cutoutFadeToBlack();
        yield return new WaitForSecondsRealtime(cutoutFadeBlack);

        SplitScreenManager.instance.DisablePlayerControls();

        yield return new WaitForSecondsRealtime(0.5f);// to stay on black screen for a second

        ResetPostProcess();

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

        foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        {
            player.GetComponent<PlayerBodyFSM>().DisablePlayerCamera();
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

    public void ResetPostProcess()
    {
        if (!postProfile.TryGet(out colorAdj)) throw new System.NullReferenceException(nameof(colorAdj));
        colorAdj.colorFilter.Override(baseFilter);

        if (!postProfile.TryGet(out dof)) throw new System.NullReferenceException(nameof(dof));
        dof.focalLength.Override(1f);
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
