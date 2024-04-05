using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ModifierVariables : MonoBehaviour
{

    [SerializeField]
    GameObject[] BonusPlatformsLavaEvent;
    [SerializeField]
    internal Transform centralLocation;

    [SerializeField]
    internal GameObject tntObject;

    [SerializeField]
    internal moveOnEvent[] eventMovables;
    [SerializeField]
    internal enableOnEvent[] eventActivateables;

    [SerializeField]
    private Transform rainPoint;

    [SerializeField]
    private GameObject rainTnTCrate;

    [SerializeField]
    private GameObject[] TNTCrates;

    [SerializeField]
    private float rainTimer = 8f;

    [SerializeField]
    private int tntAmountPerWave = 10;

    private IEnumerator rainCoro;

    private bool isRaining = false;

    private void Start()
    {
        ModifierManager.instance.vars = this;
        foreach (moveOnEvent move in eventMovables)
        {
            move.Start();
        }
        rainCoro = rainEvent();
        EventManager.instance.addListener(Events.onRoundStart, onRoundStart);
    }

    internal void onRoundStart(EventParams param = new EventParams())
    {
        for (int i=0; i<TNTCrates.Length; i++)
        {
            TNTCrates[i].SetActive(true);//.GetComponent<Explosion>().ResetTnt();
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

    internal void toggleRain(bool enabled)
    {
        StopCoroutine(rainCoro);
        if(enabled)
        {
            rainCoro = rainEvent();
            StartCoroutine(rainCoro);
        }
        else
        {
            foreach (Transform child in rainPoint.transform)
            {
                Destroy(child.gameObject);
            }
            //remove all crates spawned in
        }
    }

    internal void toggleMegaGun(bool enabled)
    {
        if (enabled)
        {
            StartCoroutine(spawnMegaGun());
        }
    }

    IEnumerator spawnMegaGun()
    {
        yield return new WaitForSeconds(5);
        Instantiate(ModifierManager.instance.MegaGunPickupPrefab, centralLocation.position, centralLocation.rotation, GunManager.instance.transform);
    }

    IEnumerator rainEvent()
    {
        float timer = 0f;

        while(true)
        {
            timer += Time.deltaTime;
            if(timer >= rainTimer)
            {

                for (int i = 0; i < SplitScreenManager.instance.GetPlayerCount(); i++)
                {
                    SplitScreenManager.instance.GetPlayers(i).playerUI.tntRainUI();
                }

                timer = 0;
                //instantiate new crate
                for(int i = 0; i < tntAmountPerWave;i++)
                {
                    float xpos = Random.Range(-30, 30);
                    float zPos = Random.Range(-30, 30);
                    Instantiate(rainTnTCrate, new Vector3(rainPoint.position.x + xpos, rainPoint.position.y, rainPoint.position.z + zPos), Quaternion.identity, rainPoint);
                }


            }
            yield return null;
        }
    }
}

[System.Serializable]
struct enableOnEvent
{
    [SerializeField]
    internal GameObject enable;
    [SerializeField]
    internal ModifierManager.RoundModifierList eventThisIsActiveIn;
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
