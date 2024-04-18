using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net;
using Unity.VisualScripting;

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

    [SerializeField] private GameObject MovingCrownUI;
    [SerializeField] private GameObject crownObject;

    [SerializeField] private Animator crownAnim;

    [SerializeField] private RectTransform[] crownSpots;

    private List<GameObject> spawnedCrownObjects;

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

        spawnedCrownObjects = new List<GameObject>();

    }

    private void Update()
    {
        UpdateRoundTimer();
    }

    public void ShowRoundTimer(EventParams param = new EventParams())
    {
        //VictoryObject.SetActive(false);
        //foreach (Transform child in VictoryObject.transform.GetChild(0))
        //{
        //    Destroy(child.gameObject);
        //}
        roundTimerGO.SetActive(true);
    }

    public void StartCrownSequence()
    {
        crownObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        crownObject.SetActive(true);
        crownAnim.Play("CrownScale");
    }

    public void RoundVictoryCrownFly(List<int> winners)
    {

        int[] pastWinners = GameManager.instance.GetRoundsWon();

        crownObject.SetActive(false);
        if(winners == null || winners.Count == 4 || winners.Contains(-1))
        {
            for(int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
            {
                GameObject newCrown = Instantiate(MovingCrownUI, VictoryObject.transform);
                spawnedCrownObjects.Add(newCrown);
                //PlayerBodyFSM newPLayer = SplitScreenManager.instance.GetPlayers(playerID);
                newCrown.GetComponent<CrownMoverUI>().InitializeEndPoint(crownSpots[i], pastWinners[i], i);
            }
        }
        else
        {
            foreach (int playerID in winners)
            {
                GameObject newCrown = Instantiate(MovingCrownUI, VictoryObject.transform);
                spawnedCrownObjects.Add(newCrown);
                //PlayerBodyFSM newPLayer = SplitScreenManager.instance.GetPlayers(playerID);
                newCrown.GetComponent<CrownMoverUI>().InitializeEndPoint(crownSpots[playerID], pastWinners[playerID], playerID);
            }
        }

    }

    public void HideTimerObject()
    {
        roundTimerGO.SetActive(false);
    }

    public void HideRoundTimer(EventParams param = new EventParams())
    {
        roundTimerGO.SetActive(false);
    }

    public void SetRoundDisplayString()
    {
        roundDisplayText.text = "Round: " + (RoundManager.instance.getRoundNum() + 1) + " / " + GameManager.instance.maxRoundsPlayed;
        if (GameManager.instance.judgeMode) roundDisplayText.text = "Round: " + (RoundManager.instance.getRoundNum() - 4) + " / " + (GameManager.instance.maxRoundsPlayed - 5);
    }

    private void UpdateRoundTimer()
    {
        float time = RoundManager.instance.getRoundTime();
        if (time <= 15) { roundTimer.color = Color.red; }
        else { roundTimer.color = Color.white; }
        if (time < 10)
        {
            roundTimer.fontSize = 72;
        }
        else
        {
            roundTimer.fontSize = 52;
        }
        roundTimer.text = Mathf.FloorToInt(time).ToString();
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

    private IEnumerator CrownSpawn()
    {
        crownObject.SetActive(true);

        float timer = 0f;

        while (timer < 0.25f)
        {
            timer += Time.unscaledDeltaTime;

            float ratio = timer / 0.5f;

            float newScale = Mathf.Lerp(0, 0.503999949f, ratio);

            newScale = easeOutBack(newScale);

            crownObject.GetComponent<RectTransform>().localScale = new Vector3(newScale, newScale, newScale);

            yield return null;
        }
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

    //called on game end and when returned to 
    public void RemoveAllCrownUI()
    {
        foreach(GameObject crown in spawnedCrownObjects)
        {
            Destroy(crown);
        }
    }

    private float easeOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3* Mathf.Pow(x - 1, 3) + c1* Mathf.Pow(x - 1, 2);
    }

}
