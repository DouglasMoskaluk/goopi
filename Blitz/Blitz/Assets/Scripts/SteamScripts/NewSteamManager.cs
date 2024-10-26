using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAchivement { OneMatch, FiveMatches, TwentyKills, HundredKills, Hammer, MegaGun, Crown, Net, Lava, Impulse, FourRounds, Platinum }
public class NewSteamManager : MonoBehaviour
{
    public static NewSteamManager instance;
    private uint appID = 2905940;
    private int totalAchievementNum = 12;
    private bool connectedToSteam = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        try
        {
            Steamworks.SteamClient.Init(appID);
            connectedToSteam = true;
            CheckForPlatinumAchievement();
        }
        catch (System.Exception e)
        {
            connectedToSteam = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Z))
        //{
        //    UnlockAchievement(eAchivement.FourRounds);

        //}

        if (GameManager.instance.ALLOW_KEYBOARD_DEVKEYS && Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Clear Achievement");
            ResetAllAchievements();
        }

        if (connectedToSteam)
        {
            Steamworks.SteamClient.RunCallbacks();
        }
    }

    public void DisconnectFromSteam()
    {

    }

    public void UnlockAchievement(eAchivement AchievementToUnlock)
    {
        var ach = new Steamworks.Data.Achievement("New_Achievement_" + (int)AchievementToUnlock);
        ach.Trigger();

        Steamworks.SteamUserStats.StoreStats();

        CheckForPlatinumAchievement();

    }

    public void AddMatch()
    {
        var steamStat = new Steamworks.Data.Stat("TotalGames");
        steamStat.Add(1);
        Steamworks.SteamUserStats.StoreStats();
    }

    public void AddKill()
    {
        var steamStat = new Steamworks.Data.Stat("TotalKills");
        steamStat.Add(1);
        Steamworks.SteamUserStats.StoreStats();
    }

    public bool MainMenuPlatCheck()
    {
        var ach = new Steamworks.Data.Achievement("New_Achievement_" + 11);

        if (ach.State)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForPlatinumAchievement()
    {
        int numOfRequired = totalAchievementNum - 1;
        int numOfUnlocked = 0;

        for(int i = 0; i < numOfRequired;i++)
        {
            var ach = new Steamworks.Data.Achievement("New_Achievement_" + i);
            if(ach.State)
            {
                numOfUnlocked++;
            }
        }

        if(numOfUnlocked == numOfRequired)
        {
            var ach = new Steamworks.Data.Achievement("New_Achievement_" + (int)eAchivement.Platinum);
            ach.Trigger();
            Steamworks.SteamUserStats.StoreStats();
        }


    }

    public void ResetAllAchievements()
    {
        for(int i = 0; i < totalAchievementNum;i++)
        {
            var ach = new Steamworks.Data.Achievement("Achievement_" + i);
            ach.Clear();
        }

        Steamworks.SteamUserStats.ResetAll(true);


    }

}
