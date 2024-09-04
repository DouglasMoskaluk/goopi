using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAchivement { test }
public class NewSteamManager : MonoBehaviour
{
    public static NewSteamManager instance;
    private uint appID = 2905940;
    private int totalAchievementNum = 1;
    private bool connectedToSteam = false;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
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
        }
        catch(System.Exception e)
        {
            connectedToSteam = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            UnlockAchievement(eAchivement.test);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
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
    }

    public void ResetAllAchievements()
    {
        for(int i = 0; i < totalAchievementNum;i++)
        {
            var ach = new Steamworks.Data.Achievement("Achievement_" + i);
            ach.Clear();
        }
    }

}
