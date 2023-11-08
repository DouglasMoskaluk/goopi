using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoCount;

    [SerializeField]
    private TextMeshProUGUI health;

    [SerializeField]
    private TextMeshProUGUI killCount;

    [SerializeField]
    private TextMeshProUGUI grenadeCount;

    [SerializeField]
    private PlayerGrenadeThrower grenade;

    [SerializeField]
    private PlayerBodyFSM player;

    [SerializeField]
    private Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grenadeCount.text = grenade.HeldGrenadeCount.ToString();
        health.text = player.Health.ToString();
        ammoCount.text = gun.Ammo.ToString();
    }
}
