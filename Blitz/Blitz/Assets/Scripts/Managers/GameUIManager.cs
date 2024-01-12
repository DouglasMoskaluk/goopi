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
        roundTimerGO.SetActive(true);
    }

    public void HideRoundTimer(EventParams param = new EventParams())
    {
        roundTimerGO.SetActive(false);
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

    public IEnumerator FadeInCoroutine(float duration)
    {
        if (duration < 0) { Debug.LogError("Trying to fade with negative number"); yield break; }

        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            //Debug.Log(timeElapsed);
            fadePanel.color = new Color(0, 0, 0, Mathf.Clamp01(timeElapsed / duration));
            yield return null;
        }

        fading = false;
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {

        if (duration < 0) { Debug.LogError("Trying to fade with negative number"); yield break; }

        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            //Debug.Log(timeElapsed);
            fadePanel.color = new Color(0, 0, 0, 1 - Mathf.Clamp01(timeElapsed / duration));
            yield return null;
        }

        fading = false;
    }
}
