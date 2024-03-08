using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour
{
    // Start is called before the first frame update

    private MainMenu mainMenu;

    private void Start()
    {
        mainMenu = transform.parent.GetComponent<MainMenu>();
    }

    public void callMainMenuScript()
    {
        mainMenu.AfterSplashAnim();
    }

    public void afterInteract()
    {
        mainMenu.FullInteractibility();
    }

}
