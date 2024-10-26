using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.VFX;
using System.Runtime.CompilerServices;

public class PlayerUIHandler : MonoBehaviour
{

    private MultiplayerEventSystem eventhandler;

    [SerializeField]
    private TextMeshProUGUI ammoCount;

    [SerializeField]
    private TextMeshProUGUI health;

    [SerializeField]
    private TextMeshProUGUI killCount;

    [SerializeField]
    private Image killIcon;

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
    private GameObject[] hitDirection;

    [SerializeField]
    private GameObject obliteratedUI;

    [SerializeField]
    private Animation lavaWarning;

    [SerializeField]
    private GameObject killMarker;

    [SerializeField]
    private GameObject characterChoice;

    private IEnumerator hitMarkerCoroutine;

    private IEnumerator gotHitCoroutine;

    public GameObject lowHealthUI;

    [SerializeField]
    private GameObject damagedUI;

    [SerializeField]
    private GameObject healingUI;

    [SerializeField]
    private GameObject nerfPickupUI;

    [SerializeField]
    private GameObject impulsePickupUI;

    [SerializeField]
    private GameObject hammerUI;

    public GameObject scaleObject;

    [SerializeField]
    private PlayerCamInput playerCam;

    [SerializeField]
    private GameObject crossHair;

    [SerializeField]
    private GameObject[] charButtons;

    [SerializeField]
    private GameObject victoryText;

    [SerializeField]
    private GameObject noAmmoNerf;

    [SerializeField]
    private GameObject stars;

    [SerializeField]
    internal IEnumerator hammerCR;

    [SerializeField]
    private Animation MegaGunDroppedUI;

    public GameObject charTaken;

    private IEnumerator[] hitDirectionIndicators;

    int kills = 0;

    [HideInInspector]
    public int playerID;

    public Sprite animalHeadSprite;

    [SerializeField] private Image reloadingIcon;

    [SerializeField] private Animation TNTRainAnim;

    private Coroutine reloadFading;

    private bool isReloadFadeRunning = false;

    private IEnumerator killCounterCoRo;

    //[SerializeField] private VisualEffect healing;

    private Material healed;
    private Material nerf;
    private Material impulse;
    [Header("Vignette Shaders")]
    [SerializeField] private float vignetteRadiusMaxValue = 1;
    [SerializeField] private float vignetteFadeInTime = 0.1f;
    [SerializeField] private float vignetteFadeOutTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        eventhandler = transform.GetChild(1).GetComponent<MultiplayerEventSystem>();

        //RoundManager.instance.onRoundReset.AddListener(resetPlayerUI);
        EventManager.instance.addListener(Events.onRoundStart, resetPlayerUI);
        EventManager.instance.addListener(Events.onGameEnd, resetPlayerUI);
        EventManager.instance.addListener(Events.onPlayerRespawn, resetPlayerUIOnDeath);

        //lowHealthUI = transform.GetChild(1).gameObject;
        //damagedUI = transform.GetChild(2).gameObject;

        //scaleObject = transform.GetChild(0).gameObject;

        //scaleObject.transform.localPosition = UILocations[playerID];
        hitMarkerCoroutine = ShowHitMarker();
        hitMarker.SetActive(false);
        killMarker.SetActive(false);
        lowHealthUI.SetActive(false);
        damagedUI.SetActive(false);
        crossHair.SetActive(false);
        obliteratedUI.SetActive(false);
        victoryText.SetActive(false);
        noAmmoNerf.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            hitDirection[i].SetActive(false);
        }

        killCounterCoRo = showKillCount();

        //set scale for 2 player here
        //check p


        StartCoroutine(setCharButton());

        //vignetteTotalTime /= 2;
    }

    public void SetUILocation(Vector2 newPos)
    {
        scaleObject.GetComponent<RectTransform>().localPosition = newPos;
    }

    public void SetScale(float scale)
    {
        lowHealthUI.transform.localScale = new Vector3(scale, 1, 1);
        damagedUI.transform.localScale = new Vector3(scale, 1, 1);
        healingUI.transform.localScale = new Vector3(scale, 1, 1);
        nerfPickupUI.transform.localScale = new Vector3(scale, 1, 1);
        impulsePickupUI.transform.localScale = new Vector3(scale, 1, 1);
    }

    public void SetReloadIndicatorColour(Color c)
    {
        reloadingIcon.color = c;
    }

    private void Initialize()
    {
        //scaleObject.transform.localPosition = UILocations[playerID];
        hitMarkerCoroutine = ShowHitMarker();
        hitMarker.SetActive(false);
        killMarker.SetActive(false);
        lowHealthUI.SetActive(false);
        damagedUI.SetActive(false);
        victoryText.SetActive(false);
        obliteratedUI.SetActive(false);
        hammerUI.SetActive(false);
        healingUI.SetActive(false);
        nerfPickupUI.SetActive(false);
        impulsePickupUI.SetActive(false);
        killMarker.SetActive(false);
        noAmmoNerf.SetActive(false);
        killCount.transform.gameObject.SetActive(false);
        killIcon.transform.gameObject.SetActive(false);
        if (stars != null) stars.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            hitDirection[i].SetActive(false);
        }
    }

    public void ReEnablePlayerUI()
    {

    }

    private void resetPlayerUIOnDeath(EventParams param)
    {
        if (param.killed == playerID)
        {
            resetPlayerUI();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        grenadeCount.text = grenade.HeldGrenadeCount.ToString();
        health.text = player.Health.ToString();
        if (gun != null && gun.Ammo < 100) ammoCount.text = gun.Ammo.ToString();
        else if (gun != null) ammoCount.text = "\u221E";
        killCount.text = RoundManager.instance.getKillCount(playerID).ToString();
    }

    public void resetPlayerUI(EventParams param = new EventParams())
    {
        scaleObject.SetActive(true);
        StopAllCoroutines();
        Initialize();
    }

    public void CharacterButtonSelected()
    {
        LockerRoomManager.instance.roomPistons[playerID].LowerPiston();
    }

    /// <summary>
    /// called on button press in character choice
    /// </summary>
    public void PickCharacter()
    {
        if(LockerRoomManager.instance.SkinIsAvailable(player.playerID))
        {
            //player.enabled = true;
            //Debug.Log("CHARACTER SELECTED");
            crossHair.SetActive(true);

            charTaken.SetActive(false);

            Alive();

            disableStars();

            //eventhandler.currentSelectedGameObject.transform.GetComponent<Button>().interactable = false;

            characterChoice.SetActive(false);
        }
        else
        {
            StartCoroutine(takenTextCoRo());
            //Debug.Log("cant pick it");
            // do taken text
        }

    }

    private IEnumerator takenTextCoRo()
    {
        charTaken.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        charTaken.SetActive(false);

    }

    public void SetSprite(Sprite newSprite)
    {
        animalHeadSprite = newSprite;
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

    internal void megaGunDropped()
    {
        MegaGunDroppedUI.Play();
    }

    internal void lavaRising()
    {
        lavaWarning.Play();
    }

    internal void tntRainUI()
    {
        TNTRainAnim.Play();

    }

    internal void playerGotKill()
    {
        kills++;
        StopCoroutine(killCounterCoRo);
        killCounterCoRo = showKillCount();
        StartCoroutine(killCounterCoRo);
        StartCoroutine("ShowKillMarker");
    }

    internal void playerGotHit()
    {
        StopCoroutine("ShowHitMarker");
        StartCoroutine("ShowHitMarker");
    }

    internal void playerGotDamaged()
    {
        StopCoroutine("ShowDamageEffect");
        StartCoroutine("ShowDamageEffect");
    }

    internal void playerGotHealed()
    {
        StopCoroutine("ShowHealEffect");
        StartCoroutine("ShowHealEffect");
    }

    public void NerfAmmoPickedup()
    {
        StopCoroutine("ShowNerfEffect");
        StartCoroutine("ShowNerfEffect");
    }

    public void ImpulsePickedup()
    {
        StopCoroutine("ShowImpulseEffect");
        StartCoroutine("ShowImpulseEffect");
    }


    public void Obliterated()
    {
        //NewSteamManager.instance.UnlockAchievement(eAchivement.MegaGun);
        StopCoroutine("ObliteratedCR");
        StartCoroutine("ObliteratedCR");
    }

    public void Hammered()
    {
        //NewSteamManager.instance.UnlockAchievement(eAchivement.Hammer);

        if (hammerCR != null) StopCoroutine(hammerCR);
        hammerCR = Hammer();
        StartCoroutine(hammerCR);
    }

    public void NerfAmmo()
    {
        StopCoroutine("NerfOutOfAmmo");
        StartCoroutine("NerfOutOfAmmo");
    }

    public void enableStars()
    {
        if (stars != null) stars.SetActive(true);
    }

    public void disableStars()
    {
        Destroy(stars);
    }

    internal void StopDamagedCoroutine()
    {
        StopAllCoroutines();

    }

    internal void showVictoryText()
    {
        //victoryText.SetActive(true);
    }

    internal void setCrosshair(Sprite crosshair)
    {
        crossHair.GetComponent<Image>().overrideSprite = crosshair;
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
        //killCount.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        crossHair.SetActive(false);
        healingUI.SetActive(false);
        nerfPickupUI.SetActive(false);
        impulsePickupUI.SetActive(false);
        killCount.transform.gameObject.SetActive(false);
        killIcon.transform.gameObject.SetActive(false);
        //healing.pause = true;

        for (int i=0; i<SplitScreenManager.instance.GetPlayerCount(); i++)
        {
            hitDirection[i].SetActive(false);
        }

    }

    internal void bulletCollision(Transform bul, int hitID)
    {
        hitDirection[hitID].SetActive(true);
        if (hitDirectionIndicators == null) hitDirectionIndicators = new IEnumerator[4];
        Vector3 pos = bul.position - player.transform.position; //get the difference in position
        Vector2 directionShot = new Vector2(pos.x, pos.z); // get the topdown view on a vector 2
        //Debug.Log(directionShot + ", " + pos + " " + bul.position + " "+ player.transform.position);
        if (hitDirectionIndicators[hitID] != null) StopCoroutine(hitDirectionIndicators[hitID]);
        hitDirectionIndicators[hitID] = pointToHitDirection(directionShot, hitID);
        StartCoroutine(hitDirectionIndicators[hitID]);

        
    }

    public void setReloadIndicatorPercent(float percent)
    {
        reloadingIcon.fillAmount = Mathf.Clamp01(percent);
    }

    public void setReloadIndicatorVisible(bool onOff)
    {
        reloadingIcon.gameObject.SetActive(onOff);
    }

    public void StopReloadFade()
    {
        if (reloadFading != null) StopCoroutine(reloadFading);
    }

    public void fadeOutReloadIcon()
    {
        if (reloadFading == null)
        {
            reloadFading = StartCoroutine(fadeReloadIcon());
        }
        else
        {
            StartCoroutine(startReloadIconFade());
        }
    }

    private IEnumerator startReloadIconFade()
    {
        StopCoroutine(reloadFading);
        yield return null;
        reloadFading = StartCoroutine(fadeReloadIcon());
    }

    private IEnumerator fadeReloadIcon()
    {
        float elapsedTime = 0;
        Color baseColor = reloadingIcon.color;
        const float fadeTime = 0.25f;
        while (elapsedTime <= fadeTime)
        {
            elapsedTime += Time.deltaTime;
            Color newCol = baseColor;
            newCol.a = Mathf.Lerp(baseColor.a, 0, Mathf.Clamp01(elapsedTime / fadeTime));
            reloadingIcon.color = newCol;
            yield return null;
        }
        reloadingIcon.color = baseColor;
        setReloadIndicatorVisible(false);
        reloadFading = null;
        isReloadFadeRunning = false;
    }

    internal IEnumerator pointToHitDirection(Vector2 pos, int hitID)
    {
        RectTransform rectTrans = hitDirection[hitID].GetComponent<RectTransform>();
        Image img = hitDirection[hitID].GetComponent<Image>();
        for (float i=0; i<2; i+= Time.deltaTime)
        {
            //Get the position, rotate it based on what direction the player is looking, then apply a distance to it 
            rectTrans.localPosition = (Quaternion.Euler(0, 0, Mathf.Rad2Deg * player.playerBody.rotation.ToEulerAngles().y) * pos).normalized * 350;
            //Rotate based on the angle of where it is
            rectTrans.rotation = Quaternion.FromToRotation(Vector2.up, rectTrans.localPosition);
            float alpha = Mathf.Min((2 - (i * 1))/2, 1);
            Color c = img.color;
            img.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        hitDirection[hitID].SetActive(false);
    }

    internal void Alive()
    {
        lowHealthUI.SetActive(false);
        ammoCount.gameObject.SetActive(true);
        grenadeCount.gameObject.SetActive(true);
        crossHair.SetActive(true);
        //killCount.gameObject.SetActive(true);
        obliteratedUI.SetActive(false);
        //health.gameObject.SetActive(true);
    }

    //Update with new HD2 inspired killfeed
    IEnumerator ShowKillMarker()
    {
        hitMarker.SetActive(false);
        killMarker.SetActive(true);

        float timer = 0;
        float ratio;
        Image img = killMarker.GetComponent<Image>();
        while(timer < 0.5f)
        {
            timer += Time.deltaTime;
            ratio = timer / 0.5f;

            img.color = new Color(1, 1, 1, 1 - ratio);

            yield return null;
        }

        //yield return new WaitForSeconds(0.5f);
        killMarker.SetActive(false);
    }

    IEnumerator ShowHitMarker()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        hitMarker.SetActive(false);
        yield return null;
    }

    IEnumerator ObliteratedCR()
    {
        obliteratedUI.SetActive(true);
        for (int i=0; i<obliteratedUI.transform.childCount; i++)
        {
            Animation anim = obliteratedUI.transform.GetChild(i).GetComponent<Animation>();
            if (anim != null) anim.Play();
        }
        yield return new WaitForSeconds(0.25f);
        obliteratedUI.SetActive(false);
        yield return null;
    }

    IEnumerator Hammer()
    {
        yield return new WaitForSeconds(0.1f);
        hammerUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hammerUI.SetActive(false);
        hammerCR = null;
    }

    IEnumerator ShowDamageEffect()
    {
        damagedUI.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        damagedUI.SetActive(false);
        yield return null;
    }

    IEnumerator NerfOutOfAmmo()
    {
        noAmmoNerf.SetActive(true);
        noAmmoNerf.GetComponent<Animator>().StopPlayback();
        //noAmmoNerf.GetComponent<Animator>().playbackTime = 0;
        noAmmoNerf.GetComponent<Animator>().Play("Pick Up Ammo", 0, 0);
        yield return new WaitForSeconds(1);
        noAmmoNerf.SetActive(false);
    }

    IEnumerator ShowHealEffect()
    {

        if(healed == null)
        {
            Image vignetteImage = healingUI.GetComponent<Image>();
            healed = Instantiate(vignetteImage.material);
            vignetteImage.material = healed;
        }

        //healing.Reinit();
        //healing.pause = false;

        healed.SetFloat("_VignetteRadiusPower", 0);

        healingUI.SetActive(true);

        float elapsedTime = 0;

        while (elapsedTime < vignetteFadeInTime)
        {
            elapsedTime += Time.deltaTime;

            float healingRadius = Mathf.Lerp(0, vignetteRadiusMaxValue, (elapsedTime / vignetteFadeInTime));

            healed.SetFloat("_VignetteRadiusPower", healingRadius);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0;

        while (elapsedTime < vignetteFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float healingRadius = Mathf.Lerp(vignetteRadiusMaxValue, 0f, (elapsedTime / vignetteFadeOutTime));

            healed.SetFloat("_VignetteRadiusPower", healingRadius);
            yield return null;
        }

        //yield return new WaitForSeconds(0.25f);
        healingUI.SetActive(false);
        yield return null;
    }

    IEnumerator ShowNerfEffect()
    {

        if (nerf == null)
        {
            Image vignetteImage = nerfPickupUI.GetComponent<Image>();
            nerf = Instantiate(vignetteImage.material);
            vignetteImage.material = nerf;
        }


        nerf.SetFloat("_VignetteRadiusPower", 0f);

        nerfPickupUI.SetActive(true);


        float elapsedTime = 0;

        while (elapsedTime < vignetteFadeInTime)
        {
            elapsedTime += Time.deltaTime;

            float nerfRadius = Mathf.Lerp(0f, vignetteRadiusMaxValue, (elapsedTime / vignetteFadeInTime));

            nerf.SetFloat("_VignetteRadiusPower", nerfRadius);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0;

        while (elapsedTime < vignetteFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float nerfRadius = Mathf.Lerp(vignetteRadiusMaxValue, 0f, (elapsedTime / vignetteFadeOutTime));

            nerf.SetFloat("_VignetteRadiusPower", nerfRadius);
            yield return null;
        }

        //yield return new WaitForSeconds(0.25f);
        nerfPickupUI.SetActive(false);
        yield return null;
    }

    IEnumerator ShowImpulseEffect()
    {

        if (impulse == null)
        {
            Image vignetteImage = impulsePickupUI.GetComponent<Image>();
            impulse = Instantiate(vignetteImage.material);
            vignetteImage.material = impulse;
        }


        impulse.SetFloat("_VignetteRadiusPower", 0f);


        impulsePickupUI.SetActive(true);


        float elapsedTime = 0;

        while (elapsedTime < vignetteFadeInTime)
        {
            elapsedTime += Time.deltaTime;

            float ImpulseRadius = Mathf.Lerp(0f, vignetteRadiusMaxValue, (elapsedTime / vignetteFadeInTime));

            impulse.SetFloat("_VignetteRadiusPower", ImpulseRadius);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0;

        while (elapsedTime < vignetteFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float ImpulseRadius = Mathf.Lerp(vignetteRadiusMaxValue, 0f, (elapsedTime / vignetteFadeOutTime));

            impulse.SetFloat("_VignetteRadiusPower", ImpulseRadius);
            yield return null;
        }

        //yield return new WaitForSeconds(0.25f);
        impulsePickupUI.SetActive(false);
        yield return null;
    }

    IEnumerator showKillCount()
    {
        killIcon.color = new Color(killIcon.color.r, killIcon.color.g, killIcon.color.b, 1f);        
        killIcon.transform.gameObject.SetActive(true);
        killCount.faceColor = new Color32(killCount.faceColor.r, killCount.faceColor.g, killCount.faceColor.b, 255);
        killCount.transform.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        float timer = 0.0f;

        while(timer < 0.5f)
        {
            timer += Time.deltaTime;
            float ratio = timer / 0.5f;
            float lerpFloat = Mathf.Lerp(1f, 0f, ratio);
            killIcon.color = new Color(killIcon.color.r, killIcon.color.g, killIcon.color.b, lerpFloat);
            killCount.faceColor = new Color32(killCount.faceColor.r, killCount.faceColor.g, killCount.faceColor.b, (byte)(lerpFloat * 255));

            yield return null;
        }
        killCount.transform.gameObject.SetActive(false);
        killIcon.transform.gameObject.SetActive(false);

    }

    IEnumerator setCharButton()
    {
        yield return new WaitForSeconds(0.05f);

        characterChoice.SetActive(true);
        eventhandler.SetSelectedGameObject(charButtons[playerID]);

        yield return null;

    }

}
