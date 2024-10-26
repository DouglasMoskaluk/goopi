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
    //[SerializeField]
    //VolumeProfile lowGravPPVolume;

    private ColorAdjustments colorAdj;

    [SerializeField]
    private Color baseFilter;

    [SerializeField]
    private Color lowGravFilter;

    internal ModifierVariables vars;
    [SerializeField]
    Transform modifierUI;

    [SerializeField]
    internal Volume globalVolume;

    [SerializeField]
    private RoundModifierList predeterminedEvent = RoundModifierList.RANDOM_GUNS;



    RoundModifierList[] modifierOrder;


    internal int getNumEvents(int r)
    {
        if (r < 0) return 0;
        return modifiers[r];
    }

    internal void showModifierUI()
    {
        /*int playedEventAudio = 0;
        for (int i = 0; i < ModifierManager.instance.ActiveEvents.Length; i++)
        {
            if (ModifierManager.instance.ActiveEvents[i])
            {
                if (i == (int)ModifierManager.RoundModifierList.RICOCHET)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_RICOCHET, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.LOW_GRAVITY)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_LOWGRAV, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.RANDOM_GUNS)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_MEGA, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.FLOOR_IS_LAVA)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_LAVA, playedEventAudio * 2);
                }
                else if (i == (int)ModifierManager.RoundModifierList.BOMB)
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_BOMB, playedEventAudio * 2);
                }
                playedEventAudio++;
            }
        }*/


        //if ((RoundManager.instance.getRoundNum() - 1 < modifiers.Length) && RoundManager.instance.getRoundNum() - 1 >= 0) Debug.Log((RoundManager.instance.getRoundNum() - 1 < modifiers.Length) + " && " + (RoundManager.instance.getRoundNum() - 1 >= 0) + " && (" + (modifiers[RoundManager.instance.getRoundNum() - 1] >= 1) + " || " + (modifiers[RoundManager.instance.getRoundNum() - 1] == -1) + ")");
        if ((RoundManager.instance.getRoundNum() - 1 < modifiers.Length && RoundManager.instance.getRoundNum() - 1 >= 0) && (modifiers[RoundManager.instance.getRoundNum() - 1] >= 1 || modifiers[RoundManager.instance.getRoundNum() - 1] == -1 || modifiers[RoundManager.instance.getRoundNum() - 1] == -2))
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
        if (GameManager.instance.ALLOW_KEYBOARD_DEVKEYS && Input.GetKeyDown(KeyCode.M))
        {
            changeModifier(new EventParams(1));
        }
    }

    void SetLowGravPostProcess(bool active)
    {
        if(active)
        {
            //globalVolume.profile = lowGravPPVolume;
            if (!defaultPPVolume.TryGet(out colorAdj)) throw new System.NullReferenceException(nameof(colorAdj));
            colorAdj.colorFilter.Override(lowGravFilter);
        }
        else
        {
            if (!defaultPPVolume.TryGet(out colorAdj)) throw new System.NullReferenceException(nameof(colorAdj));
            colorAdj.colorFilter.Override(baseFilter);
            //globalVolume.profile = defaultPPVolume;
        }
    }


    IEnumerator lavaWarning()
    {
        yield return new WaitForSeconds(26);
        for (int i=0; i<SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            SplitScreenManager.instance.GetPlayers(i).playerUI.lavaRising();
        }
        yield return new WaitForSeconds(15);
        for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            SplitScreenManager.instance.GetPlayers(i).playerUI.lavaRising();
        }
        yield return new WaitForSeconds(15);
        for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            SplitScreenManager.instance.GetPlayers(i).playerUI.lavaRising();
        }
        yield return new WaitForSeconds(18);
        for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            SplitScreenManager.instance.GetPlayers(i).playerUI.lavaRising();
        }
    }

    public void ResetModifiers()
    {
            startGravity = SplitScreenManager.instance.GetPlayers()[0].GetComponent<FSMVariableHolder>().GRAVITY;
            SetLowGravPostProcess(false);

            EventManager.instance.removeListener(Events.onPlayerDeath, RandomGunPlayerDeath);

            if (vars != null) vars.toggleLava(false);
            StopCoroutine("lavaWarning");

            if (vars != null) vars.toggleRain(false);

        for (int i = 0; i < ActiveEvents.Length; i++)
        {
            ActiveEvents[i] = false;
        }

        if (vars != null)
        {
            foreach (moveOnEvent move in vars.eventMovables)
            {
                move.Reset();
            }
        }
        if(vars != null)
        {
            for (int i = 0; i < vars.eventActivateables.Length; i++)
            {
                vars.eventActivateables[i].enable.SetActive(false);
            }
        }

    }

    void changeModifier(EventParams param = new EventParams())
    {
        //Resetting events
        if ((RoundManager.instance.getRoundNum() == 1 || GameManager.instance.judgeMode && RoundManager.instance.getRoundNum() == 6) && !ActiveEvents[(int)RoundModifierList.LOW_GRAVITY])
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
            StopCoroutine("lavaWarning");
        }
        if (ActiveEvents[(int)RoundModifierList.BOMB])
        {
            if (vars != null) vars.toggleRain(false);
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
            if (modifiers[round] != -1 && modifiers[round] != -2)
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
            else if (modifiers[round] == -1)
            {
                ActiveEvents[(int)predeterminedEvent] = true;
            } else
            {
                ActiveEvents[(int)RoundModifierList.FLOOR_IS_LAVA] = true;
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
        for (int i = 0; i < RoundManager.instance.getRoundNum()-1; i++)
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
            StartCoroutine("lavaWarning");
        }

        if (ActiveEvents[(int)RoundModifierList.BOMB])
        {
            if (vars != null) vars.toggleRain(true);
            //for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
            //{
            //    PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(i);
            //    Instantiate(BombPrefab, plr.transform.position, plr.transform.rotation, plr.transform);
            //}
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
            if (GameManager.instance.ALLOW_KEYBOARD_DEVKEYS && Input.GetKeyDown(KeyCode.M))
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
            for (int i=0; i<SplitScreenManager.instance.GetPlayerCount(); i++)
            {
                SplitScreenManager.instance.GetPlayers(i).playerUI.megaGunDropped();
            }
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

    }

}