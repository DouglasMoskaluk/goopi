using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownTimerAchievementCheck : MonoBehaviour
{
    float timer = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 30)
        {
            NewSteamManager.instance.UnlockAchievement(eAchivement.Crown);
        }

    }

    // Update is called once per frame
    void OnDisable()
    {

        //reset Timer
    }
}
