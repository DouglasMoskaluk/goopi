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

    private bool canInteract = false;

    private void Update()
    {
        if(Input.anyKeyDown && canInteract)
        {
            canInteract = false;
            MenuObject.transform.GetComponent<Animation>().Play("SplashToStartAnimation");
            interactText.SetActive(false);
        }
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
