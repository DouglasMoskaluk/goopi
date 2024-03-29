using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    [SerializeField] private Transform spotLightsHolder;

    public bool isTieBreaker = false;
    private int tieLowerKills = 0;
    private int tieMaxKills = 0;

    List<PlayerWinsData> winData;

    [SerializeField] private PlayerTieKillsIndicator tie1;
    [SerializeField] private PlayerTieKillsIndicator tie2;
    [SerializeField] private Image characterFace1;
    [SerializeField] private Image characterFace2;
    [SerializeField] private float tieBreakerWinAmount = 40f;

    private void Awake()
    {
        if (instance == null) instance = this;
        //winData = new List<PlayerWinsData>();
        podiumPositions = new List<Transform>(4);
        foreach (Transform child in podiumLocations)
        {
            podiumPositions.Add(child); 
        }
        //StartCoroutine(EnableExitButton());
    }

    private void Start()
    {
        //StartPodiumSequence();// not supposed to be here
    }

    private IEnumerator EnableExitButton()
    {
        yield return new WaitForSecondsRealtime(2f);
        exitButton.gameObject.SetActive(true);
        exitButton.interactable = true;
        eventSys.SetSelectedGameObject(exitButton.gameObject);
    }

    public void StartPodiumSequence()
    {

        StartCoroutine(PodiumSequence());
        
    }

    private IEnumerator PodiumSequence()
    {

        

        if (isTieBreaker)
        {
            yield return StartCoroutine(TieBreakerSequence());// add particle to end
            yield return new WaitForSecondsRealtime(1f);
        }
        else
        {
            //particle effects
            yield return new WaitForSecondsRealtime(2.5f);
        }

        

        anim.Play("ScreenRaise", 0, 0);
        yield return new WaitForSecondsRealtime(0.62f);//wait for screen up anim
        yield return new WaitForSecondsRealtime(0.5f);//looking at curtians

        setLights(true);

        //spotlights and cam zoom
        anim.Play("spotlight", 0, 0);
        yield return new WaitForSecondsRealtime(3.2f);//wait for spotlight anim to finish
        yield return new WaitForSecondsRealtime(0.75f);//hold on spotlight

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

        anim.Play("openCurtains");
        //particles


        yield return new WaitForSecondsRealtime(1f);//wait for open curtains

        yield return new WaitForSecondsRealtime(1f);//wait for 1 second to turn exit button on

        StartCoroutine(EnableExitButton());

    }

    private void Update()
    {
        eventSys.SetSelectedGameObject(exitButton.gameObject);
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

        winData = gameData;

        PlacePlayersOnPodium(gameData);

        isTieBreaker = CheckTieBreaker(gameData);

        if (isTieBreaker)
        {
            SetUpTieBreaker(gameData);
            tieBreakerImg.SetActive(true);
        }
        else
        {
            gameoverImg.SetActive(true);
        }

        

    }

    private bool CheckTieBreaker(List<PlayerWinsData> gameData)
    {
        return gameData[1].rank == 0;
    }

    private IEnumerator TieBreakerSequence()
    {
        characterFace1.sprite = (SplitScreenManager.instance.GetPlayerByID(winData[0].id).GetComponent<PlayerBodyFSM>().GetUIHandler().animalHeadSprite);
        characterFace2.sprite = (SplitScreenManager.instance.GetPlayerByID(winData[1].id).GetComponent<PlayerBodyFSM>().GetUIHandler().animalHeadSprite);

        anim.Play("TieBreakerTitleRaise");
        yield return new WaitForSecondsRealtime(5.16f);

        PlayerInputHandler p1InputHandler = SplitScreenManager.instance.GetPlayerByID(winData[0].id).GetComponent<PlayerInputHandler>();
        PlayerInputHandler p2InputHandler = SplitScreenManager.instance.GetPlayerByID(winData[1].id).GetComponent<PlayerInputHandler>();
        SplitScreenManager.instance.EnablePlayerControlsByID(winData[0].id);
        SplitScreenManager.instance.EnablePlayerControlsByID(winData[1].id);

        StartCoroutine(ReduceSlider(0.3f, 0.2f, tie1, tie2));
        StartCoroutine(ReduceSlider(0.3f, 0.2f, tie2, tie1));
        int winner = -1;

        while (winner == -1)
        {
            yield return null;

            if (p1InputHandler.UIMashPressed)
            {
                tie1.ChangeKillsDisplay(tie1.GetKillsNum() + 1);
            }

            if (p2InputHandler.UIMashPressed)
            {
                tie2.ChangeKillsDisplay(tie2.GetKillsNum() + 1);
            }

            if (tie1.AtMaxValue())
            {
                winner = 0;
            }
            else if (tie2.AtMaxValue())
            {
                winner = 1;
            }
        }

        if (winner == 1)
        {
            PlayerWinsData temp = winData[0];
            winData[0] = winData[1];
            winData[1] = temp;
            PlacePlayersOnPodium(winData);
        }

        SplitScreenManager.instance.DisablePlayerControlsByID(winData[0].id);
        SplitScreenManager.instance.DisablePlayerControlsByID(winData[1].id);

        //old tie breaker system
        /*
         * float firstPlaceDisplayedScore = 0;
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

        yield return new WaitForSecondsRealtime(1.0f);
        */

    }

    private IEnumerator ReduceSlider(float value, float timeInterval, PlayerTieKillsIndicator tie1, PlayerTieKillsIndicator tie2)
    {
        while (tie1.GetKillsNum() < tieBreakerWinAmount && tie2.GetKillsNum() < tieBreakerWinAmount)
        {
            yield return new WaitForSecondsRealtime(timeInterval);

            if (tie1.GetKillsNum() < tieBreakerWinAmount && tie2.GetKillsNum() < tieBreakerWinAmount)
            {
                tie1.ChangeKillsDisplay(tie1.GetKillsNum() - value);
            }
            
        }
    }

    private void SetUpTieBreaker(List<PlayerWinsData> gameData)
    {
        //tie1.SetAnimalSprite(SplitScreenManager.instance.GetPlayerByID(gameData[0].id).GetComponent<PlayerBodyFSM>().GetUIHandler().animalHeadSprite);
        tie1.SetKillsMax(tieBreakerWinAmount);
        //tie2.SetAnimalSprite(SplitScreenManager.instance.GetPlayerByID(gameData[1].id).GetComponent<PlayerBodyFSM>().GetUIHandler().animalHeadSprite);
        tie2.SetKillsMax(tieBreakerWinAmount);

        //testing
        //gameData.Clear();
        //gameData.Add(new PlayerWinsData(0, 4, 16, 0));
        //gameData.Add(new PlayerWinsData(1, 4, 13, 0));
        //gameData.Add(new PlayerWinsData(3, 2, 4, 0));
        //gameData.Add(new PlayerWinsData(2, 0, 7, 1));

        //old way of setting up tiebreaker
        /*
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
        */

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

        for (int i = 0; i < players.Count; i++)
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

    private void setLights(bool onOff)
    {
        foreach (Transform light in spotLightsHolder)
        {
            light.gameObject.SetActive(onOff);
        }
    }
}
