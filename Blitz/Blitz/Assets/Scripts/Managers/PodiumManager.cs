using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    private bool isTieBreaker = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        podiumPositions = new List<Transform>(4);
        foreach (Transform child in podiumLocations)
        {
            podiumPositions.Add(child); 
        }
        //SetUpTieBreaker(new List<PlayerWinsData>());//testing
        StartCoroutine(EnableExitButton());
        StartCoroutine(PodiumSequence());
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
        yield return new WaitForSecondsRealtime(0.5f);

        anim.Play("openCurtains");

        yield return new WaitForSecondsRealtime(0.1f);

        yield return new WaitForSecondsRealtime(anim.GetCurrentAnimatorStateInfo(0).length - 0.1f);

        if (isTieBreaker) yield return StartCoroutine(TieBreakerSequence());

        yield return new WaitForSecondsRealtime(1.0f);

        anim.Play("podiumCamPan");

        yield return new WaitForSecondsRealtime(0.1f);

        yield return new WaitForSecondsRealtime(anim.GetCurrentAnimatorStateInfo(0).length - 0.1f);

    }

    public void SetUpPodium(List<PlayerWinsData> gameData)
    {

        gameData = FindPlayerRanks(gameData);

        PlacePlayersOnPodium(gameData);

        SetUpTieBreaker(gameData);

        isTieBreaker = CheckTieBreaker(gameData);
    }

    private bool CheckTieBreaker(List<PlayerWinsData> gameData)
    {
        return false;
    }

    private IEnumerator TieBreakerSequence()
    {
        yield return null;
    }

    private void SetUpTieBreaker(List<PlayerWinsData> gameData)
    {
        //testing
        //gameData.Clear();
        //gameData.Add(new PlayerWinsData(0, 4, 16, 0));
        //gameData.Add(new PlayerWinsData(1, 4, 13, 0));
        //gameData.Add(new PlayerWinsData(3, 2, 4, 0));
        //gameData.Add(new PlayerWinsData(2, 0, 7, 1));

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
        }

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

            FSM.DisablePlayerCamera(true);
            FSM.RotateBody(Quaternion.LookRotation(-Vector3.forward));
            FSM.transitionState(PlayerMotionStates.Walk);
            FSM.transitionState(PlayerActionStates.Idle);
            FSM.DisablePlayerUI();
            FSM.DisableGun();
            FSM.AllowWinAnimation();
            FSM.SetWinAnimNumber(i);
            FSM.enabled = false;

            chara.enabled = true;

        }
    }

    public void SetWinnerText(string text)
    {
        winnerText.text = text;
    }

    public void OnExitButtonPressed()
    {
        GunManager.instance.destroyParentedWorldObjects();
        SplitScreenManager.instance.RemoveAllPlayers();
        AudioManager.instance.TransitionTrack("MainMenu");
        SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
    }
}
