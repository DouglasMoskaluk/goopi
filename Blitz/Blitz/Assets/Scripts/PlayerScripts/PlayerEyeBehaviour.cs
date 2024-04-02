using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyeBehaviour : MonoBehaviour
{

    [SerializeField]
    private EyeFeatureStorage[] features;

    private EyeFeatureStorage actualEyes;

    private PlayerBodyFSM player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerBodyFSM>();
    }

    internal void SetEyeFeatures()
    {
        actualEyes = features[player.modelID];
    }

    internal void SetHappy()
    {
        actualEyes.happyEyes.SetActive(true);
        actualEyes.winceEyes.SetActive(false);
        actualEyes.defaultEyes.SetActive(false);
        actualEyes.blinkEyes.SetActive(false);

    }

    internal void SetWince()
    {
        actualEyes.happyEyes.SetActive(false);
        actualEyes.winceEyes.SetActive(true);
        actualEyes.defaultEyes.SetActive(false);
        actualEyes.blinkEyes.SetActive(false);
    }

    internal void SetDefault()
    {
        actualEyes.happyEyes.SetActive(false);
        actualEyes.winceEyes.SetActive(false);
        actualEyes.defaultEyes.SetActive(true);
        actualEyes.blinkEyes.SetActive(false);
    }

    internal void SetBlink()
    {
        actualEyes.happyEyes.SetActive(false);
        actualEyes.winceEyes.SetActive(false);
        actualEyes.defaultEyes.SetActive(false);
        actualEyes.blinkEyes.SetActive(true);
    }

}
