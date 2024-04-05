using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    [SerializeField] private TextMeshProUGUI roundEndText;
    [SerializeField] private TextMeshProUGUI roundTimer;
    [SerializeField] private GameObject roundTimerGO;
    [SerializeField] private Image fadePanel;
    [SerializeField] private GameObject roundTransObjs;
    [SerializeField] private WeaponSlotMachine slotSelectionUI;
    [SerializeField] private GunTutorialAnimation gunTut;
    [SerializeField] private PlayerScore[] roundTransScores;
    [SerializeField] private RoundTransitionMotionManager roundTransMotion;
    [SerializeField] private CutoutFade cutoutFade;
    [SerializeField] private TextMeshProUGUI roundDisplayText;

    [SerializeField] private GameObject VictoryObject;

    [SerializeField] private GameObject PlayerVictoryIcon;

    private float timerTickDelay = 0;

    public bool fading { get; private set; } = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, ShowRoundTimer);
        EventManager.instance.addListener(Events.onRoundEnd, HideRoundTimer);
    }

    private void Update()
    {
        UpdateRoundTimer();
    }

    public void ShowRoundTimer(EventParams param = new EventParams())
    {
        VictoryObject.SetActive(false);
        roundTimerGO.SetActive(true);
    }

    public void RoundVictorySetPlayerIcons(List<int> winners)
    {
        foreach(int playerID in winners)
        {
            GameObject newIcon = Instantiate(PlayerVictoryIcon, VictoryObject.transform.GetChild(0));
            //PlayerBodyFSM newPLayer = SplitScreenManager.instance.GetPlayers(playerID);
            newIcon.GetComponent<Image>().sprite = SplitScreenManager.instance.GetPlayers(playerID).playerUI.animalHeadSprite;
        }

        VictoryObject.SetActive(true);

    }

    public void HideTimerObject()
    {
        roundTimerGO.SetActive(false);
        foreach (Transform child in VictoryObject.transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }
    }

    public void HideRoundTimer(EventParams param = new EventParams())
    {
        roundTimerGO.SetActive(false);
    }

    public void SetRoundDisplayString()
    {
        roundDisplayText.text = "Round: " + (RoundManager.instance.getRoundNum() + 1) + " / " + GameManager.instance.maxRoundsPlayed;
    }

    private void UpdateRoundTimer()
    {
        float time = RoundManager.instance.getRoundTime();
        if (time <= 15) { roundTimer.color = Color.red; }
        else { roundTimer.color = Color.white; }
        string minutes = (time >= 60) ? "1" : "0";
        string seconds = (time >= 60) ? ((int)(time - 60)).ToString() : ((int)time).ToString();
        if (seconds.Length < 2) seconds = "0" + seconds;
        roundTimer.text = minutes + ":" + seconds;
        timerTickDelay += Time.deltaTime;
        if (time <=5 && timerTickDelay > 1) { 
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.TIMER_TICK);
            timerTickDelay = 0;
        }
    }

    /// <summary>
    /// procedure for what happens to the game shared UI canvas when a round ends
    /// </summary>
    /// <param name="length">how long should the display be</param>
    /// <param name="displayText">what text should be displayed</param>
    /// <returns></returns>
    public IEnumerator DisplayRoundEndUI(float length, string displayText)
    {
        roundEndText.text = displayText;
        roundEndText.gameObject.SetActive(true);
        yield return new WaitForSeconds(length);
        roundEndText.gameObject.SetActive(false);
    }

    /// <summary>
    /// procedure for what happens to the game shared UI canvas when the game ends
    /// </summary>
    /// <param name="length">how long should the display be</param>
    /// <param name="displayText">what text should be displayed</param>
    /// <returns></returns>
    public IEnumerator DisplayGameEndUI(float length, string displayText)
    {
        roundEndText.text = displayText;
        roundEndText.gameObject.SetActive(true);
        yield return new WaitForSeconds(length);
        roundEndText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Coroutine FadeIn(float duration)
    {
        if (fading) return null;

        fading = true;
        return StartCoroutine(FadeInCoroutine(duration));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Coroutine FadeOut(float duration)
    {
        if (fading) return null;

        fading = true;
        return StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeInCoroutine(float duration)
    {
        if (duration < 0) { Debug.LogError("Trying to fade with negative number"); yield break; }

        float timeElapsed = fadePanel.color.a;
        float curFadePercent = fadePanel.color.a;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(curFadePercent, 1, timeElapsed / duration));
            yield return null;
        }

        fading = false;
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {

        if (duration < 0) { Debug.LogError("Trying to fade with negative number"); yield break; }

        float timeElapsed = 0;
        float curFadePercent = fadePanel.color.a;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(curFadePercent, 0, timeElapsed / duration));
            yield return null;
        }

        fading = false;
    }

    public void SetFadePanelAlpha(float alpha)
    {
        alpha = Mathf.Clamp01(alpha);
        Color newColor = fadePanel.color;
        newColor.a = alpha;
        fadePanel.color = newColor;
    }

    public void showRoundTransition() 
    {
        roundTransObjs.SetActive(true);
        AudioManager.instance.TransitionTrack("Roulette Spin");
    }

    public void hideRoundTransition()
    {
        roundTransObjs.SetActive(false);
        AudioManager.instance.TransitionTrack("InGame");
    }

    public Coroutine spinGunSelection(int gunSelected)
    {
        return slotSelectionUI.StartSelection(gunSelected);
    }

    public float playGunTutorial()
    {
        return gunTut.playGunTutorialSequence();
    }

    public float playGunTutorialMotion()
    {
        return roundTransMotion.playRoundTransitionMotion();
    }

    public void UpdateRoundTransScores()
    {
        int[]  roundsWon = GameManager.instance.GetRoundsWon();
        for (int i = 0; i < roundTransScores.Length; i++)
        {
            roundTransScores[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            roundTransScores[i].gameObject.SetActive(true);
            roundTransScores[i].SetWins(roundsWon[i]);
        }
    }

    public void ResetRoundTransitionUI()
    {
        gunTut.resetTutorial();
        roundTransMotion.ResetMotion();
    }

    public float cutoutFadeToBlack()
    {
        return cutoutFade.FadeToBlack();
    }

    public float cutoutFadeToVisible()
    {
        return cutoutFade.FadeToVisible();
    }

    public void cutoutFadeToBlackInstant()
    {
        cutoutFade.FadeToBlackInstant();
    }

    public void cutoutFadeToVisibleInstant()
    {
        cutoutFade.FadeToVisibleInstant();
    }

    public void changeVisibilityoFCutoutFade(bool onOff)
    {
        cutoutFade.SetVisibility(onOff);
    }

    public void setTutAnimInitFrame()
    {
        gunTut.setInitialFrame();
    }

    public void ResetSpinner()
    {
        slotSelectionUI.ResetSpinner();

    }

}
