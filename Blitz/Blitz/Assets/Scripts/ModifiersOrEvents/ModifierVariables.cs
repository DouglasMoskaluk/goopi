using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierVariables : MonoBehaviour
{

    [SerializeField]
    GameObject[] BonusPlatformsLavaEvent;
    [SerializeField]
    GameObject StartingMegaGunPickup;


    [SerializeField]
    internal moveOnEvent[] eventMovables;

    private void Start()
    {
        ModifierManager.instance.vars = this;
        foreach (moveOnEvent move in eventMovables)
        {
            move.Start();
        }
    }

    internal void toggleLava(bool enabled)
    {
        foreach (GameObject go in BonusPlatformsLavaEvent)
        {
            go.SetActive(enabled);
            if (go.transform.childCount > 0 && enabled)
            {
                PingPong ping = go.transform.GetChild(0).GetComponent<PingPong>();
                if (ping != null) ping.resetCoroutine();
            }
        }
    }

    internal void toggleMegaGun(bool enabled)
    {
        if (StartingMegaGunPickup != null) StartingMegaGunPickup.SetActive(enabled);
    }
}


[System.Serializable]
class moveOnEvent
{
    [SerializeField]
    internal ModifierManager.RoundModifierList eventThisMovesIn;
    [SerializeField]
    internal float height;
    internal float currentTime = 0;
    [SerializeField]
    internal float timeTaken;
    [SerializeField]
    internal Transform objectMoving;
    internal Vector3 startPos;
    internal void Start() { startPos = objectMoving.position; }

    internal void Reset()
    {
        objectMoving.position = startPos;
    }
}
