using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private EventSystem eventSys;

    [SerializeField] private GameObject mmPlayButton;
    [SerializeField] private GameObject omPlayButton;

    [SerializeField] private Button settingsApplyButton;

    [SerializeField]
    private RawImage flashbang;

    [SerializeField]
    private GameObject backGround;

    [SerializeField]
    private GameObject playButt;


    [SerializeField]
    private GameObject settingButt;

    [SerializeField]
    private GameObject quitButt;

    [SerializeField]
    private GameObject creditsButt;

    [SerializeField]
    private GameObject interactText;

    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private GameObject MenuObject;

    [SerializeField]
    private GameObject foxHolder;

    [SerializeField]
    private GameObject ragdoll;

    [SerializeField]
    private GameObject grenade;


    [SerializeField]
    private Transform GrenadeSpawnOne;

    [SerializeField]
    private Transform GrenadeSpawnTwo;

    [SerializeField]
    private Transform GrenadeSpawnThree;

    [SerializeField]
    private GameObject ragDollOne;

    [SerializeField]
    private GameObject ragDollTwo;

    [SerializeField]
    private GameObject ragDollThree;

    private bool canInteract = false;

    private bool fullyInteract = false;

    private void Update()
    {
        if(Input.anyKeyDown && canInteract)
        {
            canInteract = false;
            MenuObject.transform.GetComponent<Animation>().Play("SplashToStartAnimation");
            interactText.SetActive(false);
        }



        if(fullyInteract) //now we do some wacky shit
        {
            fullyInteract = false;
            StartCoroutine(ragDollStuff());
        }

        if(eventSys.currentSelectedGameObject == null)
        {
            eventSys.SetSelectedGameObject(playButt);
        }

    }

    private void Start()
    {
        //gameObjects = new List<GameObject>();
    }

    public void FullInteractibility()
    {
        fullyInteract = true;
    }

    public void AfterSplashAnim()
    {

        canInteract = true;

        backGround.SetActive(false);

        interactText.SetActive(true);

        crosshair.transform.GetComponent<Animation>().Play();

        foxHolder.SetActive(true);

        flashbang.CrossFadeAlpha(0f, 0.5f, false);




    }

    public void onMMPlayClicked()
    {
        mmPlayButton.GetComponent<Button>().interactable = false;
        GameManager.instance.ReadyLockerRoom();
    }

    public void onMMCreditsClicked()
    {
        SceneTransitionManager.instance.switchScene(Scenes.Credits);
    }



    public void onMMQuitClicked()
    {
        Application.Quit();
    }

    public void onMMSettingsClicked()
    {
        settingsMenu.SetActive(true);
    }

    public void onOMPlayClicked()
    {
        SceneTransitionManager.instance.switchScene(Scenes.LockerRoom);
    }

    public void onOPCancelClicked()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        eventSys.SetSelectedGameObject(mmPlayButton);
    }

    

    public void onSMReturnClicked()
    {
        settingsMenu.SetActive(false);
    }

    public void onSMApplyChangesClicked()
    {
        // Needs some way to store settings, probably dont actually need apply changes button
        // If we do use an apply changes button it should not be interactable by default, only clickable when any change is made
        Debug.Log("Stored Settings!");
        settingsApplyButton.interactable = false;
    }

    public void onButtonHover()
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.BUTTON_HOVER);
    }

    public void onButtonClick()
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.BUTTON_CLICK);
    }

    public void RemoveObjects()
    {
        //for(int i = 0; i< gameObjects.Count; i++)
        //{
        //    Destroy(gameObjects[i]);
        //}
    }

    IEnumerator ragDollStuff()
    {
        PlayerModelHandler modelOne = ragDollOne.transform.GetComponent<PlayerModelHandler>();

        PlayerModelHandler modelTwo = ragDollTwo.transform.GetComponent<PlayerModelHandler>();

        PlayerModelHandler modelThree = ragDollThree.transform.GetComponent<PlayerModelHandler>();


        modelOne.SetRagdollSkin(Random.Range(0, 4));
        modelOne.SetModel(Random.Range(0, 6));

        modelTwo.SetRagdollSkin(Random.Range(0, 4));
        modelTwo.SetModel(Random.Range(0, 6));

        modelThree.SetRagdollSkin(Random.Range(0, 4));
        modelThree.SetModel(Random.Range(0, 6));

        Instantiate(grenade, GrenadeSpawnOne.position, Quaternion.identity);

        Instantiate(grenade, GrenadeSpawnTwo.position, Quaternion.identity);

        Instantiate(grenade, GrenadeSpawnThree.position, Quaternion.identity);



        yield return null;
    }

    //IEnumerator MainMenuStuff()
    //{
    //    float timeElapsed = 0f;

    //    while (timeElapsed <= 0)
    //    {
    //        timeElapsed += Time.deltaTime;
    //        flashbang.color
    //    }

    //}

}
