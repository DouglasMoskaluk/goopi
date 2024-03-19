using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PodiumManager : MonoBehaviour
{
    public static PodiumManager instance;

    [SerializeField] private Slider[] playerScores;
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform podiumLocations;
    private List<Transform> podiumPositions;
    [SerializeField] private TextMeshProUGUI[] podiumRankNumbers;
    [SerializeField] private Transform[] playerLookAtPositions;

    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject tiePlayerPrefab;
    [SerializeField] private GameObject tieCanvas;
    private PlayerTieKillsIndicator[] tieIndicators = new PlayerTieKillsIndicator[4];
    [SerializeField] private Animator anim;

    [SerializeField] EventSystem eventSys;

    [SerializeField] private float durationOfTimeBreakerCount = 1.5f;
    [SerializeField] private GameObject tieBreakerImg;
    [SerializeField] private GameObject gameoverImg;

    public bool isTieBreaker = false;
    private int tieLowerKills = 0;
    private int tieMaxKills = 0;

    List<PlayerWinsData> winData;

    private void Awake()
    {
        if (instance == null) instance = this;
        podiumPositions = new List<Transform>(4);
        foreach (Transform child in podiumLocations)
        {
            podiumPositions.Add(child); 
        }
        //StartCoroutine(EnableExitButton());
    }

    private IEnumerator EnableExitButton()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        exitButton.interactable = true;
    }

    public void StartPodiumSequence()
    {
        StartCoroutine(PodiumSequence());
    }

    private IEnumerator PodiumSequence()
    {

        if (isTieBreaker)
        {

        }
        else
        {

        }
        anim.Play("ScreenRaise", 0, 0);
        yield return new WaitForSecondsRealtime(0.62f);


        if (isTieBreaker) { 
            yield return StartCoroutine(TieBreakerSequence());
            yield return new WaitForSecondsRealtime(1.0f);
        }

        List<PlayerInput> players = SplitScreenManager.instance.GetPlayers();

        for (int i = 0; i < winData.Count; i++)
        {

            CharacterController chara = players[winData[i].id].transform.GetComponent<CharacterController>();
            players[winData[i].id].transform.position = podiumPositions[i].position;
            players[winData[i].id].transform.rotation = podiumPositions[i].rotation;
        }
        
        foreach (PlayerInput player in SplitScreenManager.instance.GetPlayers())
        {
            player.GetComponent<PlayerBodyFSM>().AllowWinAnimation();
        }

        yield return new WaitForSecondsRealtime(0.1f);

        yield return new WaitForSecondsRealtime(anim.GetCurrentAnimatorStateInfo(0).length - 0.1f);

        //ShowWinnerText();

        yield return new WaitForSecondsRealtime(1f);

        exitButton.interactable = true;
        eventSys.SetSelectedGameObject(exitButton.transform.gameObject);

        //enable exit button

    }

    private void Update()
    {
        eventSys.SetSelectedGameObject(exitButton.transform.gameObject);
    }

    public void SetUpPodium(List<PlayerWinsData> gameData)
    {
        //FORCES A TIE BETWEEN player 1 AND 2
        /*gameData.Clear();
        gameData.Add(new PlayerWinsData(0, 4, 16, 0));
        gameData.Add(new PlayerWinsData(1, 4, 13, 1));
        gameData.Add(new PlayerWinsData(2, 2, 5, 1));
        gameData.Add(new PlayerWinsData(3, 1, 2, 1));*/


        gameData = FindPlayerRanks(gameData);

        gameData.ForEach(x => Debug.Log(x.ToString()));

        PlacePlayersOnPodium(gameData);

        isTieBreaker = CheckTieBreaker(gameData);

        if (isTieBreaker) SetUpTieBreaker(gameData);

        winData = gameData;

    }

    private bool CheckTieBreaker(List<PlayerWinsData> gameData)
    {
        return gameData[1].rank == 0;
    }

    private IEnumerator TieBreakerSequence()
    {
        //0 - min kills
        float firstPlaceDisplayedScore = 0;
        float secondPlaceDisplayedScore = 0;
        while (secondPlaceDisplayedScore < tieLowerKills)
        {
            firstPlaceDisplayedScore += Time.unscaledDeltaTime * durationOfTimeBreakerCount;
            secondPlaceDisplayedScore += Time.unscaledDeltaTime * durationOfTimeBreakerCount;

            tieIndicators[0].ChangeKillsDisplay(Mathf.FloorToInt(firstPlaceDisplayedScore));
            tieIndicators[1].ChangeKillsDisplay(Mathf.FloorToInt(secondPlaceDisplayedScore));

            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        //min kills - max kills
        while (firstPlaceDisplayedScore < tieMaxKills)
        {
            firstPlaceDisplayedScore += Time.unscaledDeltaTime * durationOfTimeBreakerCount;

            tieIndicators[0].ChangeKillsDisplay(Mathf.FloorToInt(firstPlaceDisplayedScore));

            yield return null;
        }

        //final
        //yield return new WaitForSecondsRealtime(0.5f);

       // ShowWinnerText();

        yield return new WaitForSecondsRealtime(1.0f);

        //exitButton.enabled = true;
        //eventSys.SetSelectedGameObject(exitButton.transform.gameObject);
    }

    private void SetUpTieBreaker(List<PlayerWinsData> gameData)
    {
        //testing
        //gameData.Clear();
        //gameData.Add(new PlayerWinsData(0, 4, 16, 0));
        //gameData.Add(new PlayerWinsData(1, 4, 13, 0));
        //gameData.Add(new PlayerWinsData(3, 2, 4, 0));
        //gameData.Add(new PlayerWinsData(2, 0, 7, 1));

        tieLowerKills = gameData[1].totalKills;
        tieMaxKills = Mathf.Max(gameData[0].totalKills, gameData[1].totalKills);
        float xAreaToPlace = 10;
        float offset = xAreaToPlace / 4;
        for (int i = 0; i < 2; i++)
        {
            float xPlacement = ((xAreaToPlace / 2) * i) - offset;
            Transform instance = Instantiate(tiePlayerPrefab, tieCanvas.transform).transform;
            instance.position = instance.position + Vector3.right * xPlacement;
            tieIndicators[i] = instance.GetComponent<PlayerTieKillsIndicator>();
            tieIndicators[i].SetKillsMax(tieMaxKills);
            tieIndicators[i].SetAnimalSprite(SplitScreenManager.instance.GetPlayerByID(gameData[i].id).GetComponent<PlayerBodyFSM>().GetUIHandler().animalHeadSprite);
        }

        /*WORKS BUT WE DONT NEED FUNCTIONALITY FOR MORE THAN 2 PLAYERS TO TIE
        int numPlayerTies = 0;
        foreach (PlayerWinsData player in gameData)
        {
            if (player.rank != 0)
            {
                break;
            }
            else
            {
                numPlayerTies++;
            }
        }

        int maxKills = 0;
        foreach (PlayerWinsData player in gameData)
        {
            maxKills = Mathf.Max(maxKills, player.totalKills);
        }

        float xAreaToPlace = 10;
        float offset = ((xAreaToPlace / numPlayerTies) * (numPlayerTies - 1)) / 2;
        for (int i = 0; i < numPlayerTies; i++)
        {
            float xPlacement = ((xAreaToPlace / numPlayerTies) * i) - offset;
            Transform instance = Instantiate(tiePlayerPrefab, tieCanvas.transform).transform;
            instance.position = instance.position + Vector3.right * xPlacement;
            tieIndicators[i] = instance.GetComponent<PlayerTieKillsIndicator>();
            tieIndicators[i].SetKillsMax(maxKills);
        }*/

    }

    private List<PlayerWinsData> FindPlayerRanks(List<PlayerWinsData> gameData)
    {
        int nextRank = 0;
        for (int i = 0; i < gameData.Count; i++)
        {
            if (i == gameData.Count-1) { gameData[i].rank = nextRank; break; }

            if (gameData[i].roundWins == gameData[i + 1].roundWins)
            {
                gameData[i].rank = nextRank;
            }
            else
            {
                gameData[i].rank = nextRank++;
            }
        }
        return gameData;
    }

    public void SetScores(int[] scores)
    {
        for (int i = 0; i < 4; i++)
        {
            playerScores[i].value = scores[i];
            playerScoreText[i].text = scores[i].ToString();
        }
    }

    public void PlacePlayersOnPodium(List<PlayerWinsData> gameData)
    {
        List<PlayerInput> players = SplitScreenManager.instance.GetPlayers();

        for (int i = 0; i < gameData.Count; i++)
        {
            
            CharacterController chara = players[gameData[i].id].transform.GetComponent<CharacterController>();
            PlayerBodyFSM FSM = players[gameData[i].id].transform.GetComponent<PlayerBodyFSM>();
            chara.enabled = false;
            players[gameData[i].id].transform.position = podiumPositions[i].position;
            players[gameData[i].id].transform.rotation = podiumPositions[i].rotation;

            podiumRankNumbers[i].text = (gameData[i].rank + 1).ToString();

            
            FSM.SetPlayerSpineValue(0.5f);
            //FSM.SetCameraLookAt(playerLookAtPositions[i]);
            //FSM.SetBodyRotToCamera();

            FSM.SetAlive();

            FSM.DisablePlayerCamera(true);
            FSM.RotateBody(Quaternion.LookRotation(-Vector3.forward));
            FSM.transitionState(PlayerMotionStates.Walk);
            FSM.transitionState(PlayerActionStates.Idle);
            FSM.DisablePlayerUI();
            FSM.DisableGun();
            //FSM.AllowWinAnimation();
            FSM.SetWinAnimNumber(i);
            FSM.enabled = false;

            //disable grenade arc and particles
            FSM.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
            FSM.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
            //disable crown
            FSM.SetCrownVisibility(false);

            players[gameData[i].id].transform.position = podiumPositions[i].position;
            players[gameData[i].id].transform.rotation = podiumPositions[i].rotation;

            chara.enabled = true;

        }
    }

    public void SetWinnerText(string text)
    {
        winnerText.text = text;
    }

    private void ShowWinnerText()
    {
        winnerText.gameObject.SetActive(true);
    }

    public void OnExitButtonPressed()
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.BUTTON_CANCEL);
        GunManager.instance.destroyParentedWorldObjects();
        //SplitScreenManager.instance.RemoveAllPlayers();
        AudioManager.instance.TransitionTrack("MainMenu");
        SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
    }

    public void OnButtonSelect()
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.BUTTON_HOVER);
    }
}
