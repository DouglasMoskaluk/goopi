using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class SkinSwap : MonoBehaviour
{
    // Start is called before the first frame update

    private RectTransform icons;

    [SerializeField]
    private PlayerInputHandler inputValues;

    [SerializeField]
    private MultiplayerEventSystem playerUIEvents;

    [SerializeField]
    private float speed = 50;

    [SerializeField]
    private float spacing = 200;

    public int ModelNum = 0;
    public int SkinNum = 0;

    [SerializeField]
    private PlayerModelHandler modelHandler;

    [SerializeField]
    private PlayerBodyFSM player;

    [SerializeField]
    private PlayerUIHandler playerUIHandler;

    [SerializeField]
    private Sprite[] animalSprites;

    [SerializeField]
    private Image[] outlineBG;

    private Button modelButton;

    private bool isMoving = false;

    void Start()
    {
        //inputValues = 
        modelButton = GetComponent<Button>();
        icons = transform.GetChild(0).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputValues.UIMoveInput.x != 0 && playerUIEvents.currentSelectedGameObject == transform.gameObject)
        {
            //Debug.Log(inputValues.UIMoveInput.x);
            IconMove(-inputValues.UIMoveInput.x);
        }

        modelButton.interactable = LockerRoomManager.instance.SkinIsAvailable(player.playerID);

        if(gameObject == playerUIEvents.currentSelectedGameObject)
        {
            //set sticker bg to active
            for(int i = 0; i < outlineBG.Length; i++)
            {
                outlineBG[i].enabled = true;
            }
        }
        else
        {
            for(int i = 0; i < outlineBG.Length; i++)
            {
                outlineBG[i].enabled = false;
            }
        }

    }

    public void SetSkinNum()
    {
        modelHandler.skinNum = SkinNum;
        modelHandler.SetSkinNum(SkinNum);
        LockerRoomManager.instance.playerModelSkinNumber[player.playerID].z = 0f;

        //LockerRoomManager.instance.SetPlayerModelSkinNumber(player.playerID, player.modelID, SkinNum);

        float orderNum = 0;

        for (int i = 0; i < LockerRoomManager.instance.playerModelSkinNumber.Length; i++)
        {
            if (i == player.playerID)
            {
                continue;
            }
            if (player.modelID == LockerRoomManager.instance.playerModelSkinNumber[i].x && SkinNum == LockerRoomManager.instance.playerModelSkinNumber[i].y)
            {
                orderNum = LockerRoomManager.instance.playerModelSkinNumber[i].z + 1;
            }
        }

        LockerRoomManager.instance.SetPlayerModelSkinNumber(player.playerID, player.modelID, SkinNum, (int)orderNum);
    }

    public void IconMove(float direction)
    {
        if (!isMoving && Mathf.Abs(direction) > 0.5)
        {

            //PUT SKIN SWAP AUDIO HERE
            AudioManager.instance.PlaySound(AudioManager.AudioQueue.BUTTON_HOVER);

            isMoving = true;
            //Debug.Log("UI MOVE");

            StartCoroutine(IconSwipe(direction));
        }
        
    }

    public void SetSprite()
    {
        playerUIHandler.SetSprite(animalSprites[SkinNum]);
    }

    public void setOrder()
    {

    }

    IEnumerator IconSwipe(float direction)
    {

        isMoving = true;

        float ratio = 0;
        //float tracker = 0;
        Vector2 newPos;

        if(direction > 0)
        {
            SkinNum--;
            newPos = icons.anchoredPosition + new Vector2(spacing, 0f);
            direction = 1;

        }
        else
        {
            SkinNum++;
            newPos = icons.anchoredPosition - new Vector2(spacing,0f);
            direction = -1;
        }


        if(SkinNum < 0)
        {
            SkinNum = 3;
        }
        else if(SkinNum > 3)
        {
            SkinNum = 0;
        }

        //for (int i = 0; i < LockerRoomManager.instance.playerModelSkinNumber.Length; i++)
        //{

        //}

        LockerRoomManager.instance.playerModelSkinNumber[player.playerID].z = 0f;

        //LockerRoomManager.instance.SetPlayerModelSkinNumber(player.playerID, player.modelID, SkinNum);

        float orderNum = 0;

        for (int i = 0;i < LockerRoomManager.instance.playerModelSkinNumber.Length;i++)
        {
            if(i == player.playerID)
            {
                continue;
            }
            if(player.modelID == LockerRoomManager.instance.playerModelSkinNumber[i].x && SkinNum == LockerRoomManager.instance.playerModelSkinNumber[i].y)
            {
                orderNum = LockerRoomManager.instance.playerModelSkinNumber[i].z + 1;
            }
        }

        LockerRoomManager.instance.SetPlayerModelSkinNumber(player.playerID, player.modelID, SkinNum, (int)orderNum);

        modelHandler.skinNum = SkinNum;

        modelHandler.SetSkinNum(SkinNum);

        modelHandler.SetModel(player.modelID);


        while (Mathf.Abs(ratio) <= spacing)
        {
            //Debug.Log(ratio);
            ratio += Time.deltaTime * direction * speed;
            //Debug.Log(fallTracker);

            icons.anchoredPosition += new Vector2(Time.deltaTime * direction * speed, 0f);

            //Debug.Log(ratio);
            yield return null;
        }

        icons.anchoredPosition = newPos;

        if (newPos.x == spacing)
        {
            icons.anchoredPosition = new Vector2(-spacing * 3, 0);
        }
        else if(newPos.x == -(spacing * 4))
        {
            icons.anchoredPosition = new Vector2(0, 0);
        }

        isMoving = false;

    }

}
