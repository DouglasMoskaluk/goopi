using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class LockerRoomManager : MonoBehaviour
{
    public static LockerRoomManager instance;

    //[HideInInspector]
    public Vector2[] buttonTracker;

    [SerializeField] private GameObject[] joinTexts;

    [SerializeField] private GameObject[] readyTexts;

    [SerializeField] private Transform[] pipeLocations;

    public CharacterPiston[] roomPistons;

    private bool[] readyFlags;
    private bool readied = false;

    [SerializeField]
    private float shakeStrength;

    [SerializeField]
    private float shakeLength;

    [SerializeField]
    private float rumbleStrength = 300;

    [SerializeField]
    private float rumbleLength = 0.5f;

    private GameObject[] players;

    [SerializeField]
    private float readyUpTime = 0.5f;

    [SerializeField]
    private float pullBackTime = 0.5f;

    [SerializeField]
    private float flingTime = 0.5f;

    private Vector3 newPipeOffset;

    [SerializeField]
    private GameObject blackScreen;

    [SerializeField]
    private GameObject[] joinCameras;

    [HideInInspector]
    public Vector3[] playerModelSkinNumber;

    private void Awake()
    {
        playerModelSkinNumber = new Vector3[4];
        for(int i = 0;  i < playerModelSkinNumber.Length;i++)
        {
            playerModelSkinNumber[i] = new Vector2(-1,-1);
        }

        if (instance == null) instance = this;
        readyFlags = new bool[4];
        SplitScreenManager.instance.AllowJoining();
    }

    private void Start()
    {
        players = new GameObject[4];
        for(int i = 0; i < roomPistons.Length; i++)
        {
            roomPistons[i].shakeStrength = shakeStrength;
            roomPistons[i].shakeLength = shakeLength;

            roomPistons[i].rumbleStrength = rumbleStrength;
            roomPistons[i].rumbleLength = rumbleLength;

        }

        newPipeOffset = new Vector3(0f, -0.65f, 0f);

    }

    public void SetPlayerModelSkinNumber(int playerNum, int modelNum, int skinNum, int order)
    {
        playerModelSkinNumber[playerNum] = new Vector3(modelNum, skinNum, order);
    }

    private void Update()
    {
        //DEBUGGING PURPOSES
        if (Input.GetKeyDown(KeyCode.U))
        {
            SendReadySignal();
        }
    }

    private void AllPlayersReady()
    {
        SceneTransitionManager.instance.switchScene(Scenes.Arena);
    }

    public void DisableJoinText(int index)
    {
        joinTexts[index].SetActive(false);
        Destroy(joinTexts[index]);
        Destroy(joinCameras[index]);
    }

    public void InitializePlayerRoom(int index, GameObject player)
    {
        roomPistons[index].getPlayer(player);
        players[index] = player;
    }

    public void ReadyUpPlayer(int playerID)//called when player enters pipe
    {
        IEnumerator ready = PlayerReady(playerID);
        StartCoroutine(ready);
    }

    private float LerpEaseIn(float value, float power)
    {
        value = Mathf.Pow(value, power);
        return value;
    }

    private float LerpFlip(float value)
    {
        return 1 - value;
    }

    private float LerpEaseOut(float value, float power)
    {
        return LerpFlip(LerpEaseIn(LerpFlip(value), power));
    }

    IEnumerator PlayerReady(int playerID)
    {
        //Debug.Log("Player " + playerID + " has readied up.");
        PlayerBodyFSM FSM = players[playerID - 1].GetComponent<PlayerBodyFSM>();

        FSM.transitionState(PlayerMotionStates.Walk);
        FSM.transitionState(PlayerActionStates.Idle);

        yield return null;

        FSM.enabled = false;
        CameraShake cam = players[playerID - 1].GetComponent<CameraShake>();
        cam.camInput.charSelect = 0f;
        cam.camCollider.enabled = false;
        players[playerID - 1].transform.GetChild(4).GetComponent<PlayerUIHandler>().Dead();
        players[playerID - 1].transform.GetChild(4).GetComponent<PlayerUIHandler>().HideLowHealth();

        //disable player mesh
        players[playerID - 1].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        players[playerID - 1].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);


        //set xaxis on camera to -90
        float ratio = 0;
        float rotateLength = Mathf.Abs(-90f - cam.freelook.m_XAxis.Value);
        float camStartPointX = cam.freelook.m_XAxis.Value;
        float camStartPointY = cam.freelook.m_YAxis.Value;
        Vector3 playerPos = players[playerID-1].transform.position;
        //Vector3 newPipeOffset = new Vector3(0f, -0.65f, 0f);

        float timeTracker = 0;

        //if (cam.freelook.m_XAxis.Value > -90f)
        //{
        //
            while(ratio <= 1.0f)
            {
                timeTracker += Time.deltaTime;
                ratio = timeTracker / readyUpTime;
                cam.freelook.m_XAxis.Value = Mathf.Lerp(camStartPointX, -90f, LerpEaseOut(ratio, 3));
                cam.freelook.m_YAxis.Value = Mathf.Lerp(camStartPointY, 0.5f, LerpEaseOut(ratio, 3));
                players[playerID - 1].transform.position = Vector3.Lerp(playerPos, pipeLocations[playerID-1].position, LerpEaseOut(ratio, 3));
                cam.offset.m_Offset = Vector3.Lerp(new Vector3(0.5f, 0.3f,0.0f), newPipeOffset, LerpEaseOut(ratio, 3));

            yield return null;
            }


        readyTexts[playerID - 1].SetActive(true);

        readyFlags[playerID-1] = true;
        if (!readied && CheckReadyStatus())
        {
            readied = true;
            SendReadySignal();
            //SceneTransitionManager.instance.loadScene(Scenes.Arena);
        }
        FSM.SetWalkParticles(false);
        
    }

    IEnumerator LobbyReady()
    {

        SplitScreenManager.instance.DisableJoining();

        //Debug.Log("EXIT LOBBY");

        float ratio = 0f;
        float timeTracker = 0;

        CameraShake[] cams = new CameraShake[4];

        for (int i = 0; i < 4; i++)
        {

            readyTexts[i].SetActive(false);

            if (players[i] != null)
            {
                cams[i] = players[i].GetComponent<CameraShake>();
            }
        }

        while (ratio <= 1.0f)
        {
            timeTracker += Time.deltaTime;
            ratio = timeTracker / pullBackTime;

            for(int i = 0;i < 4;i++)
            {
                if (cams[i] != null)
                {
                    cams[i].offset.m_Offset = Vector3.Lerp(newPipeOffset, new Vector3(0.0f,-0.65f,-0.4f), LerpEaseOut(ratio, 5));

                }
            }

            yield return null;
        }

        ratio = 0f;
        timeTracker = 0;

        while (ratio <= 1.0f)
        {
            timeTracker += Time.deltaTime;
            ratio = timeTracker / flingTime;

            for (int i = 0; i < 4; i++)
            {
                if (cams[i] != null)
                {
                    cams[i].offset.m_Offset = Vector3.Lerp(new Vector3(0.0f, -0.65f, -0.4f), new Vector3(0.0f, -0.65f, 2.25f), LerpEaseIn(ratio, 3));

                }
            }

            yield return null;
        }

        //enable black canvas

        yield return new WaitForSecondsRealtime(0.15f);
        blackScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(0.5f);

        //reenable playerFSM
        for(int i = 0;i<4;i++)
        {
            if (players[i] != null)
            {
                players[i].GetComponent<PlayerBodyFSM>().enabled = true;

                cams[i].camInput.charSelect = 1f;
                cams[i].camCollider.enabled = true;
                cams[i].ResetOffset();

                players[i].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                players[i].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);

                players[i].transform.GetChild(4).GetComponent<PlayerUIHandler>().Alive();


            }
        }

        yield return new WaitForEndOfFrame();

        yield return GameManager.instance.ReadyArena();

    }

    public bool SkinIsAvailable(int playerID)
    {
        for(int i = 0;i<players.Length;i++)
        {
            if(i == playerID)
            {
                continue;
            }
            if (playerModelSkinNumber[i].x == playerModelSkinNumber[playerID].x && playerModelSkinNumber[i].y == playerModelSkinNumber[playerID].y && playerModelSkinNumber[i].z < playerModelSkinNumber[playerID].z)
            {
                return false;
            }
        }
        return true;
    }

    private void SendReadySignal()//call when all players are ready
    {
        GameUIManager.instance.SetCrownSpots();
        StartCoroutine(LobbyReady());
        //GameManager.instance.ReadyArena();
    }

    private bool CheckReadyStatus()
    {
        for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            if (readyFlags[i] == false) return false;
        }
        return true;
    }


}
