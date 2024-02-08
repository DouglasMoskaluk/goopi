using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class CharArrowFollow : MonoBehaviour
{
    [SerializeField]
    private MultiplayerEventSystem eventSystem;

    [SerializeField]
    private RectTransform rect;

    private void Start()
    {
     //   rect = transform.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (eventSystem.currentSelectedGameObject)
        {
            rect.anchoredPosition = eventSystem.currentSelectedGameObject.transform.GetComponent<RectTransform>().anchoredPosition;
        }
    }

}
