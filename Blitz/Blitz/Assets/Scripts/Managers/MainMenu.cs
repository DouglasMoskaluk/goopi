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

    [SerializeField]
    private GameObject floorPlane;

    [SerializeField]
    private GameObject settingReturnButt;

    [SerializeField]
    private GameObject[] MMObjects;

    private bool isSettings = false;

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

        if(eventSys.currentSelectedGameObject == null && isSettings == false)
        {
            eventSys.SetSelectedGameObject(playButt);
        }
        else if(eventSys.currentSelectedGameObject == null && isSettings)
        {
            eventSys.SetSelectedGameObject(settingReturnButt);
        }

    }

    private void Awake()
    {
        SplitScreenManager.instance.RemoveAllPlayers();
        GameUIManager.instance.HideTimerObject();
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
        //mainMenu.SetActive(false);

        //set setting bool here
        settingsMenu.SetActive(true);
        for(int i = 0; MMObjects.Length > i; i++)
        {
            MMObjects[i].SetActive(false);
        }
        isSettings = true;
        settingsMenu.GetComponent<OptionsMenu>().loadSettings();
        eventSys.SetSelectedGameObject(settingReturnButt);
    }

    public void onOMPlayClicked()
    {
        SceneTransitionManager.instance.switchScene(Scenes.LockerRoom);
    }

    public void onOPCancelClicked()
    {
        for (int i = 0; MMObjects.Length > i; i++)
        {
            MMObjects[i].SetActive(true);
        }
        optionsMenu.SetActive(false);
        eventSys.SetSelectedGameObject(mmPlayButton);
    }

    

    public void onSMReturnClicked()
    {
        //mainMenu.SetActive(true);
        settingsMenu.GetComponent<Animator>().SetBool("SM", false);
        for(int i = 0; MMObjects.Length > i; i++)
        {
            MMObjects[i].SetActive(true);
        }
        isSettings = true;
        eventSys.SetSelectedGameObject(playButt);
        isSettings = false;
    }

    public void onSMApplyChangesClicked()
    {
        // Needs some way to store settings, probably dont actually need apply changes button
        // If we do use an apply changes button it should not be interactable by default, only clickable when any change is made
        //Debug.Log("Stored Settings!");
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

    public void onButtonCancel()
    {
        AudioManager.instance.PlaySound(AudioManager.AudioQueue.BUTTON_CANCEL);
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


        int numOne;

        int numTwo;

        int numThree;

        numOne = Random.Range(0, 6);

        numTwo = Random.Range(0, 6);

        while(numOne == numTwo)
        {
            numTwo = Random.Range(0, 6);
        }

        numThree = Random.Range(0, 6);

        while(numThree == numOne || numThree == numTwo)
        {
            numThree = Random.Range(0, 6);
        }

        modelOne.SetRagdollSkin(0);
        modelOne.SetModel(numOne);

        modelTwo.SetRagdollSkin(0);
        modelTwo.SetModel(numTwo);

        modelThree.SetRagdollSkin(0);
        modelThree.SetModel(numThree);

        Instantiate(grenade, GrenadeSpawnOne.position, Quaternion.identity);

        Instantiate(grenade, GrenadeSpawnTwo.position, Quaternion.identity);

        Instantiate(grenade, GrenadeSpawnThree.position, Quaternion.identity);

        float timeTracker = 0.0f;

        while(timeTracker < 1.15f)
        {
            timeTracker += Time.deltaTime;
            yield return null;
        }

        timeTracker = 0.0f;

        Rigidbody modelRB1 = ragDollOne.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Rigidbody>();
        Rigidbody modelRB2 = ragDollTwo.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Rigidbody>();
        Rigidbody modelRB3 = ragDollThree.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Rigidbody>();

        floorPlane.SetActive(false);

        while (timeTracker < 0.85)
        {
            timeTracker += Time.deltaTime;
            ragDollOne.GetComponent<MainMenuDragAdd>().SetDrag(timeTracker * 7.5f);
            ragDollTwo.GetComponent<MainMenuDragAdd>().SetDrag(timeTracker * 7.5f);
            ragDollThree.GetComponent<MainMenuDragAdd>().SetDrag(timeTracker * 7.5f);

            yield return null;
        }

        ragDollOne.GetComponent<MainMenuDragAdd>().VelocityOff();
        ragDollTwo.GetComponent<MainMenuDragAdd>().VelocityOff();
        ragDollThree.GetComponent<MainMenuDragAdd>().VelocityOff();

        ragDollOne.GetComponent<MainMenuDragAdd>().SetDrag(0.0f);
        ragDollOne.GetComponent<MainMenuDragAdd>().SetDrag(0.0f);
        ragDollOne.GetComponent<MainMenuDragAdd>().SetDrag(0.0f);

        ragDollOne.GetComponent<MainMenuDragAdd>().FreezePosition();
        ragDollTwo.GetComponent<MainMenuDragAdd>().FreezePosition();
        ragDollThree.GetComponent<MainMenuDragAdd>().FreezePosition();

        yield return null;

        //ragDollOne.GetComponent<MainMenuDragAdd>().SetDrag(0);
        //ragDollTwo.GetComponent<MainMenuDragAdd>().SetDrag(0);
        //ragDollThree.GetComponent<MainMenuDragAdd>().SetDrag(0);

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
