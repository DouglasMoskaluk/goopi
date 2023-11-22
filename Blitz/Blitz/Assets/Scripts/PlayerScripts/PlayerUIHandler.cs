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

    [SerializeField]
    private GameObject hitMarker;

    [SerializeField]
    private GameObject killMarker;

    private IEnumerator hitMarkerCoroutine;

    private IEnumerator gotHitCoroutine;

    private GameObject lowHealthUI;

    private GameObject damagedUI;

    [SerializeField] private TextMeshProUGUI roundTimerText;

    int kills = 0;

    public int playerID;

    // Start is called before the first frame update
    void Start()
    {
        hitMarkerCoroutine = ShowHitMarker();
        hitMarker.SetActive(false);
        killMarker.SetActive(false);
        lowHealthUI = transform.GetChild(1).gameObject;
        damagedUI = transform.GetChild(2).gameObject;
        lowHealthUI.SetActive(false);
        damagedUI.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        grenadeCount.text = grenade.HeldGrenadeCount.ToString();
        health.text = player.Health.ToString();
        if (gun != null) ammoCount.text = gun.Ammo.ToString();
        killCount.text = RoundManager.instance.GetKillCount(playerID).ToString();
        UpdateRoundTimer();
    }

    internal void UpdateRoundTimer()
    {
        float time = (int)RoundManager.instance.GetRoundTime();
        string minutes = (time >= 60) ? "1" : "0";
        string seconds = (time >= 60) ? ((int)(time - 60)).ToString() : ((int)time).ToString();
        if (seconds.Length < 2) seconds = "0" + seconds;
        roundTimerText.text = minutes + ":" + seconds;
    }

    internal void playerGotKill()
    {
        kills++;
        StartCoroutine("ShowKillMarker");
    }

    internal void playerGotHit()
    {
        StopCoroutine("ShowHitMarker");
        StartCoroutine("ShowHitMarker");
    }

    internal void playerGotdamaged()
    {
        StopCoroutine("ShowDamageEffect");
        StartCoroutine("ShowDamageEffect");
    }

    internal void StopDamagedCoroutine()
    {
        StopAllCoroutines();
    }

    internal void ShowLowHealth()
    {
        lowHealthUI.SetActive(true);
    }

    internal void HideLowHealth()
    {
        lowHealthUI.SetActive(false);
        damagedUI.SetActive(false);
    }

    IEnumerator ShowKillMarker()
    {
        killMarker.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        killMarker.SetActive(false);
        yield return null;
    }

    IEnumerator ShowHitMarker()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        hitMarker.SetActive(false);
        yield return null;
    }

    IEnumerator ShowDamageEffect()
    {
        damagedUI.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        damagedUI.SetActive(false);
        yield return null;
    }

}
