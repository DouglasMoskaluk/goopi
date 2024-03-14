using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AddPlayerVolume : MonoBehaviour
{
    [SerializeField]
    private PlayerBodyFSM player;
    // Start is called before the first frame update
    private void Start()
    {
        ModifierManager.instance.playerCamVolumes[player.playerID] = transform.GetComponent<Volume>();
    }
}
