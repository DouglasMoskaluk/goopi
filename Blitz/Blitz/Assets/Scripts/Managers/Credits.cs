using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
    }
}
