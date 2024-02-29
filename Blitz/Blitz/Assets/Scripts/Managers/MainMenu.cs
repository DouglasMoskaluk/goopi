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

    public void onMMPlayClicked()
    {
        mmPlayButton.GetComponent<Button>().interactable = false;
        GameManager.instance.ReadyLockerRoom();
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
}
