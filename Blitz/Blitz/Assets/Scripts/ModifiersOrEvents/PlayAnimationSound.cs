using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationSound : MonoBehaviour
{
    // Start is called before the first frame update
    void playSound(int soundNum)
    {
        switch(soundNum)
        {
            case 0:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_RICOCHET);
                break;
            case 1:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_LOWGRAV);
                break;
            case 2:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_BOMB);
                break;
            case 3:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_LAVA);
                break;
            case 4:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ANNOUNCE_MEGA);
                break;
        }
    }
}
