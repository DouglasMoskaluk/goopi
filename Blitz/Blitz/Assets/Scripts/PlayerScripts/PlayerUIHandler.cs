using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

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
    private GameObject hammerUI;

    private GameObject scaleObject;

    [SerializeField]
    private Vector3[] UILocations;

    [SerializeField]
    private PlayerCamInput playerCam;

    [SerializeField]
    private GameObject crossHair;

    [SerializeField]
    private GameObject[] charButtons;

    [SerializeField]
    private GameObject victoryText;

    private IEnumerator[] hitDirectionIndicators;

    int kills = 0;

    [HideInInspector]
    public int playerID;

    public Sprite animalHeadSprite;

    // Start is called before the first frame update
    void Start()
    {
        eventhandler = transform.GetChild(1).GetComponent<MultiplayerEventSystem>();

        //RoundManager.instance.onRoundReset.AddListener(resetPlayerUI);
        EventManager.instance.addListener(Events.onRoundStart, resetPlayerUI);

        //lowHealthUI = transform.GetChild(1).gameObject;
        //damagedUI = transform.GetChild(2).gameObject;

        scaleObject = transform.GetChild(0).gameObject;

        scaleObject.transform.localPosition = UILocations[playerID];
        hitMarkerCoroutine = ShowHitMarker();
        hitMarker.SetActive(false);
        killMarker.SetActive(false);
        lowHealthUI.SetActive(false);
        damagedUI.SetActive(false);
        crossHair.SetActive(false);
        obliteratedUI.SetActive(false);
        victoryText.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            hitDirection[i].SetActive(false);
        }

        StartCoroutine(setCharButton());
    }

    private void Initialize()
    {
        scaleObject.transform.localPosition = UILocations[playerID];
        hitMarkerCoroutine = ShowHitMarker();
        hitMarker.SetActive(false);
        killMarker.SetActive(false);
        lowHealthUI.SetActive(false);
        damagedUI.SetActive(false);
        victoryText.SetActive(false);
        obliteratedUI.SetActive(false);
        hammerUI.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            hitDirection[i].SetActive(false);
        }
    }

    public void ReEnablePlayerUI()
    {

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
        //player.enabled = true;
        Debug.Log("CHARACTER SELECTED");
        crossHair.SetActive(true);

        Alive();

        //eventhandler.currentSelectedGameObject.transform.GetComponent<Button>().interactable = false;

        characterChoice.SetActive(false);
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


    public void Obliterated()
    {
        StopCoroutine("ObliteratedCR");
        StartCoroutine("ObliteratedCR");
    }

    public void Hammered()
    {
        StopCoroutine("Hammer");
        StartCoroutine("Hammer");
    }

    internal void StopDamagedCoroutine()
    {
        StopAllCoroutines();

    }

    internal void showVictoryText()
    {
        victoryText.SetActive(true);
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
        killCount.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        crossHair.SetActive(false);
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
        killCount.gameObject.SetActive(true);
        obliteratedUI.SetActive(false);
        //health.gameObject.SetActive(true);
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

    IEnumerator ObliteratedCR()
    {
        obliteratedUI.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        obliteratedUI.SetActive(false);
        yield return null;
    }

    IEnumerator Hammer()
    {
        yield return new WaitForSeconds(0.25f);
        hammerUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        hammerUI.SetActive(false);
        yield return null;
    }

    IEnumerator ShowDamageEffect()
    {
        damagedUI.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        damagedUI.SetActive(false);
        yield return null;
    }

    IEnumerator ShowHealEffect()
    {
        healingUI.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        healingUI.SetActive(false);
        yield return null;
    }

    IEnumerator setCharButton()
    {
        yield return new WaitForSeconds(0.05f);

        characterChoice.SetActive(true);
        eventhandler.SetSelectedGameObject(charButtons[playerID]);

        yield return null;

    }

}
