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
        //actualEyes = features[player.modelID];
    }

    internal void SetHappy()
    {
        features[player.modelID].happyEyes.SetActive(true);
        features[player.modelID].winceEyes.SetActive(false);
        features[player.modelID].defaultEyes.SetActive(false);
        features[player.modelID].blinkEyes.SetActive(false);

    }

    internal void SetWince()
    {
        features[player.modelID].happyEyes.SetActive(false);
        features[player.modelID].winceEyes.SetActive(true);
        features[player.modelID].defaultEyes.SetActive(false);
        features[player.modelID].blinkEyes.SetActive(false);
    }

    internal void SetDefault()
    {
        features[player.modelID].happyEyes.SetActive(false);
        features[player.modelID].winceEyes.SetActive(false);
        features[player.modelID].defaultEyes.SetActive(true);
        features[player.modelID].blinkEyes.SetActive(false);
    }

    internal void SetBlink()
    {
        features[player.modelID].happyEyes.SetActive(false);
        features[player.modelID].winceEyes.SetActive(false);
        features[player.modelID].defaultEyes.SetActive(false);
        features[player.modelID].blinkEyes.SetActive(true);
    }

}
