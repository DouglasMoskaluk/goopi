using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    [SerializeField] private TextMeshProUGUI roundEndText;
    [SerializeField] private TextMeshProUGUI roundTimer;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    private void Update()
    {
        UpdateRoundTimer();
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
}
