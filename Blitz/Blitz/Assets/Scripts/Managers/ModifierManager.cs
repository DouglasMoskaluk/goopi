using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    internal static ModifierManager instance;

    [SerializeField]
    private int[] modifiers;
    internal enum RoundModifierList
    {
        RICOCHET,
        LOW_GRAVITY,
        //BOMB,
        //FLOOR_IS_LAVA,
        RANDOM_GUNS,
        LENGTH
    }

    internal bool[] ActiveEvents;

    float startGravity;
    [SerializeField]
    float GravityEventGravity = 10;
    [SerializeField]
    GameObject MegaGunPickupPrefab;


    void initEvents(EventParams param = new EventParams())
    {
        //Resetting events
        if (RoundManager.instance.getRoundNum() == 1)
        {
            startGravity = SplitScreenManager.instance.GetPlayers()[0].GetComponent<FSMVariableHolder>().GRAVITY;
        }
        if (ActiveEvents[(int)RoundModifierList.RANDOM_GUNS])
        {
            EventManager.instance.removeListener(Events.onPlayerDeath, RandomGunPlayerDeath);
        }
        for (int i=0; i<ActiveEvents.Length; i++)
        {
            ActiveEvents[i] = false;
        }

        //Selects an event
        int round = RoundManager.instance.getRoundNum() - 1;
        if (modifiers[round] != -1)
        {
            for (int i = 0; i < modifiers[round]; i++)
            {
                int chosenEvent = Random.Range(0, (int)RoundModifierList.LENGTH -1);
                if (chosenEvent <= (int)RoundModifierList.LENGTH && !ActiveEvents[chosenEvent])
                {
                    ActiveEvents[chosenEvent] = true;
                }
                else if (ActiveEvents[chosenEvent])
                {
                    i--;
                }
            }
        } else
        {
            ActiveEvents[(int)RoundModifierList.RANDOM_GUNS] = true;
        }

        //Low gravity event
        if (ActiveEvents[(int)RoundModifierList.LOW_GRAVITY])
        {
            for (int i = 0; i < SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                SplitScreenManager.instance.GetPlayers()[i].GetComponent<FSMVariableHolder>().GRAVITY = GravityEventGravity;
            }
        } else
        {
            for (int i=0; i< SplitScreenManager.instance.GetPlayers().Count; i++)
            {
                SplitScreenManager.instance.GetPlayers()[i].GetComponent<FSMVariableHolder>().GRAVITY = startGravity;
            }
        }

        // Random Gun event
        if (ActiveEvents[(int)RoundModifierList.RANDOM_GUNS])
        {
            EventManager.instance.addListener(Events.onPlayerDeath, RandomGunPlayerDeath);
        }
        /*
        int playedEventAudio = 0;
        for (int i = 0; i < ModifierManager.instance.ActiveEvents.Length; i++)
        {
            if (ModifierManager.instance.ActiveEvents[i])
            {
                if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RICOCHET])
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_RICOCHET, playedEventAudio * 2);
                }
                else if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.LOW_GRAVITY])
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_LOWGRAV, playedEventAudio * 2);
                }
                else if (ModifierManager.instance.ActiveEvents[(int)ModifierManager.RoundModifierList.RANDOM_GUNS])
                {
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_MEGA, playedEventAudio * 2);
                }
                playedEventAudio++;
            }
        }*/

    }


    void RandomGunPlayerDeath(EventParams param = new EventParams())
    {
        PlayerBodyFSM died = SplitScreenManager.instance.GetPlayers(param.killed);
        if (died.playerGun.gunVars.type == Gun.GunType.BOOMSTICK)
        {
            Instantiate(MegaGunPickupPrefab, died.transform.position, Quaternion.identity, GunManager.instance.transform);
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
        if (modifiers.Length < GameManager.instance.maxRoundsPlayed)
        {
            int[] temp = new int[GameManager.instance.maxRoundsPlayed];
            for (int i = 0; i < modifiers.Length; i++)
            {
                temp[i] = modifiers[i];
            }
        }
        EventManager.instance.addListener(Events.onRoundEnd, initEvents, 0);
    }





}
