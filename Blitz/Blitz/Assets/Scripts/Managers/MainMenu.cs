using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private EventSystem eventSys;

    [SerializeField] private GameObject mmPlayButton;
    [SerializeField] private GameObject omPlayButton;

    public void onMMPlayClicked()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        eventSys.SetSelectedGameObject(omPlayButton);
        AudioManager.instance.TransitionTrack("InGame");
    }

    public void onMMQuitClicked()
    {
        Application.Quit();
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
}
