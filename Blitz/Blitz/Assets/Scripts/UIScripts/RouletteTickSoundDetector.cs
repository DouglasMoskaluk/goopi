using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RouletteTickSoundDetector : MonoBehaviour
{
    private List<RumbleHandler> playerRumbles;

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("UIRouletteSpinner"))
        {
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROULETTE_SPIN);
            //play rumble on all controllers
            List<PlayerInput> newPlayerInputList = SplitScreenManager.instance.GetPlayers();
            for (int i = 0; i < newPlayerInputList.Count; i++)
            {
                newPlayerInputList[i].transform.GetComponent<RumbleHandler>().RumbleTick();
            }
        }
    }

}
