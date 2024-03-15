using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ModifierManager : MonoBehaviour
{
    internal static ModifierManager instance;

    [SerializeField]
    private int[] modifiers;
    internal enum RoundModifierList
    {
        RICOCHET,
        LOW_GRAVITY,
        BOMB,
        FLOOR_IS_LAVA,
        RANDOM_GUNS,
        LENGTH
    }

    internal bool[] ActiveEvents;

    float startGravity;
    [SerializeField]
    float GravityEventGravity = 10;
    [SerializeField]
    internal GameObject MegaGunPickupPrefab;
    [SerializeField]
    GameObject BombPrefab;
    [SerializeField]
    VolumeProfile defaultPPVolume;
    [SerializeField]
    VolumeProfile lowGravPPVolume;

    internal ModifierVariables vars;
    [SerializeField]
    Transform modifierUI;

    [HideInInspector]
    public Volume[] playerCamVolumes;



    RoundModifierList[] modifierOrder;


    internal int getNumEvents(int r)
    {
        if (r < 0) return 0;
        return modifiers[r];
    }

    internal void showModifierUI()
    {
        //if ((RoundManager.instance.getRoundNum() - 1 < modifiers.Length) && RoundManager.instance.getRoundNum() - 1 >= 0) Debug.Log((RoundManager.instance.getRoundNum() - 1 < modifiers.Length) + " && " + (RoundManager.instance.getRoundNum() - 1 >= 0) + " && (" + (modifiers[RoundManager.instance.getRoundNum() - 1] >= 1) + " || " + (modifiers[RoundManager.instance.getRoundNum() - 1] == -1) + ")");
        if ((RoundManager.instance.getRoundNum() - 1 < modifiers.Length && RoundManager.instance.getRoundNum() - 1 >= 0) && (modifiers[RoundManager.instance.getRoundNum() - 1] >= 1 || modifiers[RoundManager.instance.getRoundNum() - 1] == -1))
        {
            int shownUI = 0;
            Transform[] uiElements = new Transform[2];
            int elemPlaced = 0;
            for (int i = 0; i < ActiveEvents.Length; i++)
            {
                if (ActiveEvents[i])
                {
                    uiElements[elemPlaced] = modifierUI.GetChild(i).GetChild(shownUI);
                    elemPlaced++;
                    shownUI++;
                    if (shownUI > 1)
                    {
                        break;
                    }
                    else if (shownUI == 1 && (modifiers[RoundManager.instance.getRoundNum() - 1] == 1 || modifiers[RoundManager.instance.getRoundNum() - 1] == -1))
                    {
                        uiElements[0] = modifierUI.GetChild(i).GetChild(2);
                        break;
                    }
                }
            }
            modifierUI.gameObject.SetActive(true);
            for (int i = 0; i < uiElements.Length; i++)
            {
                if (uiElements[i] != null) uiElements[i].gameObject.SetActive(true);
            }
        }
    }


    internal void hideModifierUI()
    {
        for (int i=0; i< ActiveEvents.Length; i++)
        {
            modifierUI.GetChild(i).GetChild(0).gameObject.SetActive(false);
            modifierUI.GetChild(i).GetChild(1).gameObject.SetActive(false);
            modifierUI.GetChild(i).GetChild(2).gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            changeModifier(new EventParams(1));
        }
    }

    void SetLowGravPostProcess(bool active)
    {
        if(active)
        {
            for(int i = 0; i < 4; i++)
            {
                if (playerCamVolumes[i] != null)
                {
                    playerCamVolumes[i].profile = lowGravPPVolume;
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerCamVolumes[i] != null)
                {
                    playerCamVolumes[i].profile = defaultPPVolume;
                }
            }
        }
    }

    void changeModifier(EventParams param = new EventParams())
    {
        //Resetting events
        if (RoundManager.instance.getRoundNum() == 1 && !ActiveEvents[(int)RoundModifierList.LOW_GRAVITY])
        {
            startGravity = SplitScreenManager.instance.GetPlayers()[0].GetComponent<FSMVariableHolder>().GRAVITY;
            SetLowGravPostProcess(false);
        }
        if (ActiveEvents[(int)RoundModifierList.RANDOM_GUNS])
        {
            EventManager.instance.removeListener(Events.onPlayerDeath, RandomGunPlayerDeath);
        }
        if (ActiveEvents[(int)RoundModifierList.FLOOR_IS_LAVA])
        {
            if (vars != null) vars.toggleLava(false);
        }
        for (int i=0; i<ActiveEvents.Length; i++)
        {
            ActiveEvents[i] = false;
        }
        foreach (moveOnEvent move in vars.eventMovables)
        {
            move.Reset();
        }
        for (int i=0; i<vars.eventActivateables.Length; i++)
        {
            vars.eventActivateables[i].enable.SetActive(false);
        }
        EventManager.instance.invokeEvent(Events.onEventEnd);


        if (param.killed == 0) // the default attacker
        {
            int round = RoundManager.instance.getRoundNum() - 1;
            if (modifiers[round] != -1)
            {
                for (int i = 0; i < modifiers[round]; i++)
                {
                    int chosenEvent;
                    do
                    {
                        chosenEvent = Random.Range(0, (int)RoundModifierList.LENGTH - 1);
                    } while (repeatEvent(chosenEvent));
                    if (chosenEvent <= (int)RoundModifierList.LENGTH && !ActiveEvents[chosenEvent] && (round == 0 || (int)modifierOrder[round - 1] != chosenEvent))
                    {
                        ActiveEvents[chosenEvent] = true;

                        modifierOrder[round] = (RoundModifierList)chosenEvent;
                    }
                    else if (ActiveEvents[chosenEvent])
                    {
                        i--;
                    }
                }
            }
            else
            {
                ActiveEvents[(int)RoundModifierList.RANDOM_GUNS] = true;
            }
        }
        else
        {
            ActiveEvents[Random.Range(0, (int)RoundModifierList.LENGTH - 1)] = true;
        }
        //Selects an event


        Debug.Log("Events have been changed: Ricochet:" + ActiveEvents[0] + ", Low Grave:" + ActiveEvents[1] + ", bomb:" + ActiveEvents[2] + ", lava:" + ActiveEvents[3] + ", mega:" + ActiveEvents[4]);

        EventManager.instance.invokeEvent(Events.onEventStart);
    }


    bool repeatEvent(int chosenEvent)
    {
        for (int i = 0; i < RoundManager.instance.getRoundNum(); i++)
        {
            if ((int)modifierOrder[i] == chosenEvent) return true;
        }
        return false;
    }
    void ActivateModifier(EventParams param = new EventParams())
    {
        //Low gravity event
        if (ActiveEvents[(int)RoundModifierList.LOW_GRAVITY])
        {
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                SplitScreenManager.instance.GetPlayers()[i].GetComponent<FSMVariableHolder>().GRAVITY = GravityEventGravity;
                SetLowGravPostProcess(true);
            }
        }
        else
        {
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                SplitScreenManager.instance.GetPlayers()[i].GetComponent<FSMVariableHolder>().GRAVITY = startGravity;
                SetLowGravPostProcess(false);
            }
        }

        // Random Gun event
        if (ActiveEvents[(int)RoundModifierList.RANDOM_GUNS])
        {
            EventManager.instance.addListener(Events.onPlayerDeath, RandomGunPlayerDeath);
            if (vars != null) vars.toggleMegaGun(true);
        }

        //Lava rising event
        if (ActiveEvents[(int)RoundModifierList.FLOOR_IS_LAVA])
        {
            if (vars != null) vars.toggleLava(true);
        }

        if (ActiveEvents[(int)RoundModifierList.BOMB])
        {
            for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
            {
                PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(i);
                Instantiate(BombPrefab, plr.transform.position, plr.transform.rotation, plr.transform);
            }
        }

        //Event Movables
        for (int i = 0; i < vars.eventMovables.Length; i++)
        {
            if (ActiveEvents[(int)vars.eventMovables[i].eventThisMovesIn])
            {
                StartCoroutine(moveEvents(vars.eventMovables[i]));
            }
        }

        //Event Activateables
        for (int i = 0; i < vars.eventActivateables.Length; i++)
        {
            if (ActiveEvents[(int)vars.eventActivateables[i].eventThisIsActiveIn])
                vars.eventActivateables[i].enable.SetActive(true);
        }
    }

    IEnumerator moveEvents(moveOnEvent move)
    {
        while(move.currentTime < move.timeTaken)
        {
            yield return null;
            move.currentTime += Time.deltaTime;
            move.objectMoving.position = move.startPos + Vector3.up * Mathf.Lerp(0, move.height, (float)move.currentTime / move.timeTaken);
            if (Input.GetKeyDown(KeyCode.M))
            {
                move.objectMoving.position = move.startPos;
                break;
            }
        }
    }


    void RandomGunPlayerDeath(EventParams param = new EventParams())
    {
        PlayerBodyFSM died = SplitScreenManager.instance.GetPlayers(param.killed);
        if (died.playerGun.gunVars.type == Gun.GunType.BOOMSTICK)
        {
            Instantiate(MegaGunPickupPrefab, died.transform.position, Quaternion.identity, GunManager.instance.transform);
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_DROP);
            // play guitar riff sound
        }
        GunManager.instance.assignGun(param.killed, GunManager.instance.pickGun());
    }


    private void Awake()
    {
        instance = this;
        ActiveEvents = new bool[(int)RoundModifierList.LENGTH];
        for (int i = 0; i < ActiveEvents.Length; i++)
        {
            ActiveEvents[i] = false;
        }
    }



    private void Start()
    {
        modifierOrder = new RoundModifierList[GameManager.instance.maxRoundsPlayed];

        if (modifiers.Length < GameManager.instance.maxRoundsPlayed)
        {
            int[] temp = new int[GameManager.instance.maxRoundsPlayed];
            for (int i = 0; i < modifiers.Length; i++)
            {
                temp[i] = modifiers[i];
            }
        }
        EventManager.instance.addListener(Events.onRoundEnd, changeModifier, 0);
        EventManager.instance.addListener(Events.onEventStart, ActivateModifier);

        playerCamVolumes = new Volume[4];

    }

}