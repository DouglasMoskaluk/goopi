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
    internal Gun gun;

    [SerializeField]
    private GameObject hitMarker;

    [SerializeField]
    private GameObject killMarker;

    [SerializeField]
    private GameObject characterChoice;

    private IEnumerator hitMarkerCoroutine;

    private IEnumerator gotHitCoroutine;

    private GameObject lowHealthUI;

    private GameObject damagedUI;

    int kills = 0;

    public int playerID;

    // Start is called before the first frame update
    void Start()
    {
        //RoundManager.instance.onRoundReset.AddListener(resetPlayerUI);
        EventManager.instance.addListener(Events.onRoundStart, resetPlayerUI);

        lowHealthUI = transform.GetChild(1).gameObject;
        damagedUI = transform.GetChild(2).gameObject;
        Initialize();
    }

    private void Initialize()
    {
        hitMarkerCoroutine = ShowHitMarker();
        hitMarker.SetActive(false);
        killMarker.SetActive(false);
        lowHealthUI.SetActive(false);
        damagedUI.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        grenadeCount.text = grenade.HeldGrenadeCount.ToString();
        health.text = player.Health.ToString();
        if (gun != null) ammoCount.text = gun.Ammo.ToString();
        killCount.text = RoundManager.instance.getKillCount(playerID).ToString();
    }

    public void resetPlayerUI()
    {
        StopAllCoroutines();
        Initialize();
    }

    /// <summary>
    /// called on button press in character choice
    /// </summary>
    public void PickCharacter()
    {
        Debug.Log("CHAR CHOICE");
        player.enabled = true;
        characterChoice.SetActive(false);
    }

    //depreciated
    /*internal void updateRoundTimer()
    {
        float time = (int)RoundManager.instance.getRoundTime();
        string minutes = (time >= 60) ? "1" : "0";
        string seconds = (time >= 60) ? ((int)(time - 60)).ToString() : ((int)time).ToString();
        if (seconds.Length < 2) seconds = "0" + seconds;
        roundTimerText.text = minutes + ":" + seconds;
    }*/

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
        killMarker.SetActive(false);
        hitMarker.SetActive(false);
    }

    internal void Dead()
    {
        lowHealthUI.SetActive(true);
        damagedUI.SetActive(false);
        killMarker.SetActive(false);
        hitMarker.SetActive(false);
        ammoCount.gameObject.SetActive(false);
        grenadeCount.gameObject.SetActive(false);
        killCount.gameObject.SetActive(false);
        health.gameObject.SetActive(false);


    }



    internal void Alive()
    {
        lowHealthUI.SetActive(false);
        ammoCount.gameObject.SetActive(true);
        grenadeCount.gameObject.SetActive(true);
        killCount.gameObject.SetActive(true);
        health.gameObject.SetActive(true);
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
