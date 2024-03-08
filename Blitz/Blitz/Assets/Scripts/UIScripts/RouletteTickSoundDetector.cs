using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteTickSoundDetector : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("UIRouletteSpinner"))
        {
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.ROULETTE_SPIN);
            Debug.Log("PLAYED THE SOUND OH MY GOD ITS SOUNDS SO GOOD. ICE CREAM SO GOOD");
        }
    }
}
