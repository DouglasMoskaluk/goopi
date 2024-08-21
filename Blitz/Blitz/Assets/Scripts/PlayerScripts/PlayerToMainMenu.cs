using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject textObject;

    private TextMeshProUGUI text;

    private bool isCounting = false;

    private PlayerInputHandler playerInputHandler;

    private IEnumerator countdownCoro;

    void Start()
    {
        text = textObject.transform.GetComponent<TextMeshProUGUI>();
        playerInputHandler = transform.GetComponent<PlayerInputHandler>();
        countdownCoro = startCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCounting && playerInputHandler.optionsPressed == true)
        {
            isCounting = true;
            StopCoroutine(countdownCoro);
            countdownCoro = startCountdown();
            StartCoroutine(countdownCoro);
        }
    }

    IEnumerator startCountdown()
    {
        Debug.Log("start countdown");
        textObject.SetActive(true);

        float timer = 0f;
        while(timer < 3.1f && playerInputHandler.optionsPressed)
        {
            timer += Time.deltaTime;

            if(timer < 1f)
            {
                text.text = "Returning to Main Menu in 3";
            }
            else if(timer < 2)
            {
                text.text = "Returning to Main Menu in 3 2";
            }
            else if(timer < 3)
            {
                text.text = "Returning to Main Menu in 3 2 1";
            }

            if (timer > 3f)
            {
                SplitScreenManager.instance.DisableJoining();
                GameManager.instance.ResetManager();
                RoundManager.instance.ResetManager();
                ModifierManager.instance.ResetModifiers();
                GameUIManager.instance.RemoveAllCrownUI();
                RoundManager.instance.ResetPostProcess();
                SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
                SplitScreenManager.instance.Hide3PersonBlackObject();
                AudioManager.instance.TransitionTrack("MainMenu");
                Time.timeScale = 1f;
                yield break;
            }

            yield return null;

        }

        isCounting = false;
        text.text = "";
        textObject.SetActive(false);

    }

}
