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
    GameObject MegaGunPickupPrefab;
    [SerializeField]
    GameObject BombPrefab;
    internal ModifierVariables vars;
    [SerializeField]
    Transform modifierUI;

    internal int getNumEvents(int r)
    {
        return modifiers[r];
    } 


    internal IEnumerator showModifierUI()
    {
        if (RoundManager.instance.getRoundNum() < modifiers.Length && modifiers[RoundManager.instance.getRoundNum()] >= 1)
        {
            int shownUI = 0;
            Transform[] uiElements = new Transform[2];
            int elemPlaced = 0;
            for (int i=0; i<ActiveEvents.Length; i++)
            {
                if (ActiveEvents[i])
                {
                    uiElements[elemPlaced] = modifierUI.GetChild(i).GetChild(shownUI);
                    elemPlaced++;
                    shownUI++;
                    if (shownUI > 1)
                    {
                        break;
                    } else if (shownUI == 1 && modifiers[RoundManager.instance.getRoundNum()-2] == 1)
                    {
                        uiElements[elemPlaced] = modifierUI.GetChild(i).GetChild(shownUI);
                        break;
                    }
                }
            }
            modifierUI.gameObject.SetActive(true);
            for (int i = 0; i < uiElements.Length; i++)
            {
                if (uiElements[i] != null) uiElements[i].gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < uiElements.Length; i++)
            {
                if (uiElements[i] != null) uiElements[i].gameObject.SetActive(false);
            }
            modifierUI.gameObject.SetActive(false);
        }

    }


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
        if (ActiveEvents[(int)RoundModifierList.FLOOR_IS_LAVA])
        {
            if (vars != null) vars.toggleLava(false);
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
            if (vars != null) vars.toggleMegaGun(true);
        }

        //Lava rising event
        if (ActiveEvents[(int)RoundModifierList.FLOOR_IS_LAVA])
        {
            if (vars != null) vars.toggleLava(true);
        }

        if (ActiveEvents[(int)RoundModifierList.BOMB])
        {
            for (int i=0; i<SplitScreenManager.instance.GetPlayerCount(); i++)
            {
                PlayerBodyFSM plr = SplitScreenManager.instance.GetPlayers(i);
                Instantiate(BombPrefab, plr.transform.position, plr.transform.rotation, plr.transform);
            }
        }

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
