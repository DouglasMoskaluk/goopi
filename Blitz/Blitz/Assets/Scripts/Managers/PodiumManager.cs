using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PodiumManager : MonoBehaviour
{
    public static PodiumManager instance;

    [SerializeField] private Slider[] playerScores;
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    [SerializeField] private TextMeshProUGUI winnerText;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetScores(int[] scores)
    {
        for (int i = 0; i < 4; i++)
        {
            playerScores[i].value = scores[i];
            playerScoreText[i].text = scores[i].ToString();
        }
    }

    public void SetWinnerText(string text)
    {
        winnerText.text = text;
    }

    public void OnExitButtonPressed()
    {
        AudioManager.instance.TransitionTrack("MainMenu");
        SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
    }
}
