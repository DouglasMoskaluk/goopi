using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem.UI;

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


    private bool isMoving = false;

    void Start()
    {
        //inputValues = 
        icons = transform.GetChild(0).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputValues.UIMoveInput.x != 0)
        {
            //Debug.Log(inputValues.UIMoveInput.x);
            IconMove(-inputValues.UIMoveInput.x);
        }
    }

    public void IconMove(float direction)
    {
        if (!isMoving && Mathf.Abs(direction) > 0.5 && playerUIEvents.currentSelectedGameObject == transform.gameObject)
        {
            isMoving = true;
            Debug.Log("UI MOVE");

            StartCoroutine(IconSwipe(direction));
        }
        
    }

    IEnumerator IconSwipe(float direction)
    {

        isMoving = true;

        float ratio = 0;
        //float tracker = 0;
        Vector2 newPos;

        if(direction > 0)
        {
            newPos = icons.anchoredPosition + new Vector2(spacing, 0f);
            direction = 1;

        }
        else
        {
            newPos = icons.anchoredPosition - new Vector2(spacing,0f);
            direction = -1;
        }

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
            icons.anchoredPosition = new Vector2(-600, 0);
        }
        else if(newPos.x == -(spacing * 4))
        {
            icons.anchoredPosition = new Vector2(0, 0);
        }

        isMoving = false;

    }

}
