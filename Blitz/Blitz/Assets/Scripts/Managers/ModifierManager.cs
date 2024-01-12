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
        for (int i=0; i< modifiers[round]; i++)
        {
            int chosenEvent = Random.Range(0, (int)RoundModifierList.LENGTH);
            if (chosenEvent <= (int)RoundModifierList.LENGTH && !ActiveEvents[chosenEvent])
            {
                ActiveEvents[chosenEvent] = true;
            } else if (ActiveEvents[chosenEvent])
            {
                i--;
            }
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

        if (ActiveEvents[(int)RoundModifierList.RANDOM_GUNS])
        {
            EventManager.instance.addListener(Events.onPlayerDeath, RandomGunPlayerDeath);
        }

    }


    void RandomGunPlayerDeath(EventParams param = new EventParams())
    {
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
